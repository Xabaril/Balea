using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Volvoreta.Abstractions;

namespace Volvoreta.Endpoints
{
    public class VolvoretaMiddleware
    {
        private readonly RequestDelegate _next;

        public VolvoretaMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IRuntimeAuthorizationServerStore store, VolvoretaOptions options)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                var authorization = await store
                    .FindAuthorizationAsync(context.User);

                var roleClaims = authorization.Roles
                    .Where(role => role.Enabled)
                    .Select(role => new Claim(options.DefaultRoleClaimType, role.Name));

                var permissionClaims = authorization.Roles
                    .SelectMany(role => role.GetPermissions())
                    .Select(permission => new Claim(VolvoretaClaims.Permission, permission));

                var identity = new ClaimsIdentity(nameof(VolvoretaMiddleware));

                identity.AddClaims(roleClaims);
                identity.AddClaims(permissionClaims);

                if (authorization.Delegation != null)
                {
                    identity.AddClaim(new Claim(VolvoretaClaims.DelegatedBy, authorization.Delegation.Who));
                    identity.AddClaim(new Claim(VolvoretaClaims.DelegatedFrom, authorization.Delegation.From.ToString()));
                    identity.AddClaim(new Claim(VolvoretaClaims.DelegatedTo, authorization.Delegation.To.ToString()));
                }

                context.User.AddIdentity(identity);
            }

            await _next(context);
        }
    }
}
