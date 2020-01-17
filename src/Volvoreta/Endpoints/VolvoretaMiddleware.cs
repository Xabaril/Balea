using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
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
                var authorization = await store.FindAsync(context.User);
                var roles = authorization.Roles;
                var delegation = authorization.Delegation;
                var permissions = roles.SelectMany(role => role.GetPermissions());

                var identity = new ClaimsIdentity(nameof(VolvoretaMiddleware));
                identity.AddClaims(roles.Where(role => role.Enabled).Select(role => new Claim(options.DefaultRoleClaimType, role.Name)));
                identity.AddClaims(permissions.Select(permission => new Claim(Claims.Permission, permission)));

                if (delegation != null)
                {
                    identity.AddClaim(new Claim(Claims.DelegatedBy, delegation.Who));
                    identity.AddClaim(new Claim(Claims.DelegatedFrom, delegation.From.ToString()));
                    identity.AddClaim(new Claim(Claims.DelegatedTo, delegation.To.ToString()));
                }

                context.User.AddIdentity(identity);
            }

            await _next(context);
        }
    }
}
