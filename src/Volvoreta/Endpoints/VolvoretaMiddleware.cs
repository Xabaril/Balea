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
                var roles = await store.FindRolesAsync(context.User);
                var enabledRoles = roles.Where(role => role.Enabled);
                var permissions = enabledRoles.SelectMany(role => role.GetPermissions());

                var identity = new ClaimsIdentity(nameof(VolvoretaMiddleware));
                identity.AddClaims(enabledRoles.Where(role => role.Enabled).Select(role => new Claim(options.DefaultRoleClaimType, role.Name)));
                identity.AddClaims(permissions.Select(permission => new Claim(Constants.Permission, permission)));

                context.User.AddIdentity(identity);
            }

            await _next(context);
        }
    }
}
