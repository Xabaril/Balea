using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using Volvoreta.Abstractions;

namespace Volvoreta.Authorization
{
    internal class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IRuntimeAuthorizationServerStore _store;

        public PermissionAuthorizationHandler(IRuntimeAuthorizationServerStore store)
        {
            _store = store ?? throw new ArgumentNullException(nameof(store));
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                if ( await _store.HasPermissionAsync(context.User, requirement.Name))
                {
                    context.Succeed(requirement);
                    return;
                }
            }
            
            context.Fail();
        }
    }
}
