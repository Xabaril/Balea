using Balea.Abstractions;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Balea
{
    public class DefaultPermissionEvaluator : IPermissionEvaluator
    {
        private readonly BaleaOptions _options;

        public DefaultPermissionEvaluator(BaleaOptions options)
        {
            _options = options;
        }

        public Task<bool> HasPermissionAsync(ClaimsPrincipal user, string permission)
        {
            var hasPermission = user
                .GetClaimValues(_options.BaleaPermissionClaimType)
                .Any(claimValue => claimValue.Equals(permission, StringComparison.InvariantCultureIgnoreCase));

            return Task.FromResult(hasPermission);
        }
    }
}
