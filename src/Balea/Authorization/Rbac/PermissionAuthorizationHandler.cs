using Balea.Abstractions;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Balea.Authorization.Rbac
{
    internal class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IPermissionEvaluator _permissionEvaluator;

        public PermissionAuthorizationHandler(IPermissionEvaluator permissionEvaluator)
        {
            Ensure.Argument.NotNull(permissionEvaluator, nameof(permissionEvaluator));
            _permissionEvaluator = permissionEvaluator;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                if (await _permissionEvaluator.HasPermissionAsync(context.User, requirement.Name))
                {
                    context.Succeed(requirement);
                    return;
                }
            }

            context.Fail();
        }
    }
}
