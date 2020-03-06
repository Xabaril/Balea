using Balea.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Balea.Endpoints
{
    public class BaleaMiddleware
    {
        internal const string AuthorizationMiddlewareInvokedKey = "__AuthorizationMiddlewareWithEndpointInvoked";
        private readonly RequestDelegate _next;

        public BaleaMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IRuntimeAuthorizationServerStore store, BaleaOptions options)
        {
            var endpoint = context.GetEndpoint();

            if (context.User.Identity.IsAuthenticated && endpoint.Metadata.GetMetadata<IAuthorizeData>() != null)
            {
                if (context.Items.ContainsKey(AuthorizationMiddlewareInvokedKey))
                {
                    ThrowMissingAuthMiddlewareException();
                }

                var authorization = await store
                    .FindAuthorizationAsync(context.User);

                if (!context.Response.HasStarted && options.UnauthorizedFallback != null && !authorization.Roles.Any())
                {
                    await options.UnauthorizedFallback(context);

                    return;
                }

                var roleClaims = authorization.Roles
                    .Where(role => role.Enabled)
                    .Select(role => new Claim(options.DefaultClaimTypeMap.RoleClaimType, role.Name));

                var permissionClaims = authorization.Roles
                    .SelectMany(role => role.GetPermissions())
                    .Select(permission => new Claim(options.DefaultClaimTypeMap.PermissionClaimType, permission));

                var identity = new ClaimsIdentity(
                    authenticationType: nameof(BaleaMiddleware),
                    nameType: options.DefaultClaimTypeMap.NameClaimType,
                    roleType: options.DefaultClaimTypeMap.RoleClaimType);

                identity.AddClaims(roleClaims);
                identity.AddClaims(permissionClaims);

                if (authorization.Delegation != null)
                {
                    identity.AddClaim(new Claim(BaleaClaims.DelegatedBy, authorization.Delegation.Who));
                    identity.AddClaim(new Claim(BaleaClaims.DelegatedFrom, authorization.Delegation.From.ToString()));
                    identity.AddClaim(new Claim(BaleaClaims.DelegatedTo, authorization.Delegation.To.ToString()));
                }

                context.User.AddIdentity(identity);
            }

            await _next(context);
        }
        private static void ThrowMissingAuthMiddlewareException()
        {
            throw new InvalidOperationException("The call to app.UseAuthorization() must appear after app.UseBalea().");
        }

    }
}
