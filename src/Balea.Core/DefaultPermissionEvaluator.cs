using Balea.Abstractions;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Balea
{
    /// <summary>
    /// Check if BaleaMiddleware authentication scheme has claims permissions fot the current user. 
    /// </summary>
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
                .GetClaimValues(_options.ClaimTypeMap.PermissionClaimType)
                .Any(claimValue => claimValue.Equals(permission, StringComparison.InvariantCultureIgnoreCase));

            return Task.FromResult(hasPermission);
        }
    }
}
