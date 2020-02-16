using System;
using System.Collections.Generic;
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
            var sourceRoleClaims = user.GetRoleClaimValues(_options.SourceRoleClaimType);
            var application = _volvoreta.Applications.GetByName(_options.ApplicationName);
            var delegation = application.Delegations.GetCurrentDelegation(user.GetSubjectId());
            var subject = GetSubject(user, delegation);
            var roles = application.Roles
                    .Where(role => role.Subjects.Contains(subject, StringComparer.InvariantCultureIgnoreCase) || 
                                   role.Mappings.Any(m => sourceRoleClaims.Contains(m, StringComparer.InvariantCultureIgnoreCase)))
                    .Select(role => role.To());

            var authorization = new AuthotizationResult(roles, delegation.To());

            return Task.FromResult(authorization);
        }

        public Task<bool> HasPermissionAsync(ClaimsPrincipal user, string permission)
        {
            var volvoretaRoleClaims = user.GetRoleClaimValues(_options.VolvoretaRoleClaimType);
            var application = _volvoreta.Applications.GetByName(_options.ApplicationName);
            var delegation = application.Delegations.GetCurrentDelegation(user.GetSubjectId());
            var subject = GetSubject(user, delegation);

            return Task.FromResult(
                application.Roles
                    .Where(role =>
                        role.Enabled &&
                        volvoretaRoleClaims.Contains(role.Name, StringComparer.InvariantCultureIgnoreCase)
                    )
                    .SelectMany(role => role.Permissions)
                    .Contains(permission)
            );
        }


        public Task<bool> IsInRoleAsync(ClaimsPrincipal user, string role)
        {
            var application = _volvoreta.Applications.GetByName(_options.ApplicationName);
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
