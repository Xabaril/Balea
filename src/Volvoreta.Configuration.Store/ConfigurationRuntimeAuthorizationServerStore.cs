using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Volvoreta.Abstractions;
using Volvoreta.Configuration.Store.Model;
using Volvoreta.Model;

namespace Volvoreta.Configuration.Store
{
    public class ConfigurationRuntimeAuthorizationServerStore : IRuntimeAuthorizationServerStore
    {
        private readonly VolvoretaConfiguration _volvoreta;
        private readonly VolvoretaOptions _options;

        public ConfigurationRuntimeAuthorizationServerStore(VolvoretaConfiguration volvoreta, VolvoretaOptions options)
        {
            _volvoreta = volvoreta ?? throw new ArgumentNullException(nameof(volvoreta));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public Task<AuthotizationResult> FindAuthorizationAsync(ClaimsPrincipal user)
        {
            var claimsRole = user.GetClaimRoleValues(_options.DefaultRoleClaimType);
            var application = _volvoreta.Applications.GetByName(_options.DefaultApplicationName);
            var delegation = application.Delegations.GetCurrentDelegation(user.GetSubjectId());
            var subject = GetSubject(user, delegation);
            var roles = application.Roles
                    .Where(role => role.Subjects.Contains(subject) || role.Mappings.Any(m => claimsRole.Contains(m)))
                    .Select(role => role.To());

            var authorization = new AuthotizationResult(roles, delegation.To());

            return Task.FromResult(authorization);
        }

        public Task<bool> HasPermissionAsync(ClaimsPrincipal user, string permission)
        {
            var claimsRole = user.GetClaimRoleValues(_options.DefaultRoleClaimType);
            var application = _volvoreta.Applications.GetByName(_options.DefaultApplicationName);
            var delegation = application.Delegations.GetCurrentDelegation(user.GetSubjectId());
            var subject = GetSubject(user, delegation);

            return Task.FromResult(
                application.Roles
                    .Where(role =>
                        role.Enabled &&
                        (role.Subjects.Contains(subject) || role.Mappings.Any(m => claimsRole.Contains(m)))
                    )
                    .SelectMany(role => role.Permissions)
                    .Contains(permission)
            );
        }


        public Task<bool> IsInRoleAsync(ClaimsPrincipal user, string role)
        {
            var application = _volvoreta.Applications.GetByName(_options.DefaultApplicationName);
            var delegation = application.Delegations.GetCurrentDelegation(user.GetSubjectId());
            var subject = GetSubject(user, delegation);

            return Task.FromResult(
                application.Roles
                    .Any(r => 
                        r.Enabled && 
                        r.Name.Equals(role, StringComparison.InvariantCultureIgnoreCase) &&
                        (r.Subjects.Contains(subject) || r.Mappings.Any(m => m.Equals(role, StringComparison.InvariantCultureIgnoreCase)))
                    )
            );
        }

        private string GetSubject(ClaimsPrincipal user, DelegationConfiguration delegation)
        {
            return delegation?.Who ?? user.GetSubjectId();
        }
    }
}
