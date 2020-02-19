using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Balea.Abstractions;
using Balea.Configuration.Store.Model;
using Balea.Model;

namespace Balea.Configuration.Store
{
    public class ConfigurationRuntimeAuthorizationServerStore : IRuntimeAuthorizationServerStore
    {
        private readonly BaleaConfiguration _Balea;
        private readonly BaleaOptions _options;

        public ConfigurationRuntimeAuthorizationServerStore(BaleaConfiguration Balea, BaleaOptions options)
        {
            _Balea = Balea ?? throw new ArgumentNullException(nameof(Balea));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public Task<AuthotizationContext> FindAuthorizationAsync(ClaimsPrincipal user)
        {
            var sourceRoleClaims = user.GetClaimValues(_options.SourceRoleClaimType);
            var application = _Balea.Applications.GetByName(_options.ApplicationName);
            var delegation = application.Delegations.GetCurrentDelegation(user.GetSubjectId(_options.SourceNameIdentifierClaimType));
            var subject = GetSubject(user, delegation);
            var roles = application.Roles
                    .Where(role => role.Subjects.Contains(subject, StringComparer.InvariantCultureIgnoreCase) || 
                                   role.Mappings.Any(m => sourceRoleClaims.Contains(m, StringComparer.InvariantCultureIgnoreCase)))
                    .Select(role => role.To());

            var authorization = new AuthotizationContext(roles, delegation.To());

            return Task.FromResult(authorization);
        }

        public Task<bool> HasPermissionAsync(ClaimsPrincipal user, string permission)
        {
            //var roles = user.GetRoleClaimValues(_options.BaleaRoleClaimType);
            //var application = _Balea.Applications.GetByName(_options.ApplicationName);
            //var delegation = application.Delegations.GetCurrentDelegation(user.GetSubjectId(_options.SourceNameIdentifierClaimType));
            //var subject = GetSubject(user, delegation);

            //return Task.FromResult(
            //    application.Roles
            //        .Where(role =>
            //            role.Enabled &&
            //            roles.Contains(role.Name, StringComparer.InvariantCultureIgnoreCase)
            //        )
            //        .SelectMany(role => role.Permissions)
            //        .Contains(permission)
            //);

            var hasPermission = user.GetClaimValues(_options.BaleaPermissionClaimType)
                .Any(claimValue => claimValue.Equals(permission, StringComparison.InvariantCultureIgnoreCase));

            return Task.FromResult(hasPermission);
        }


        public Task<bool> IsInRoleAsync(ClaimsPrincipal user, string role)
        {
            var application = _Balea.Applications.GetByName(_options.ApplicationName);
            var delegation = application.Delegations.GetCurrentDelegation(user.GetSubjectId(_options.SourceNameIdentifierClaimType));
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
            return delegation?.Who ?? user.GetSubjectId(_options.SourceNameIdentifierClaimType);
        }
    }
}
