using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Balea.Abstractions;
using System;

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
            if (context.User.Identity.IsAuthenticated)
            {
                if (context.Items.ContainsKey(AuthorizationMiddlewareInvokedKey))
                {
                    ThrowMissingAuthMiddlewareException();
                }

                var authorization = await store
                    .FindAuthorizationAsync(context.User);

                var roleClaims = authorization.Roles
                    .Where(role => role.Enabled)
                    .Select(role => new Claim(options.BaleaRoleClaimType, role.Name));

                var permissionClaims = authorization.Roles
                    .SelectMany(role => role.GetPermissions())
                    .Select(permission => new Claim(BaleaClaims.Permission, permission));

                var identity = new ClaimsIdentity(
                    authenticationType: nameof(BaleaMiddleware),
                    nameType: options.BaleaNameClaimType,
                    roleType: options.BaleaRoleClaimType);

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
