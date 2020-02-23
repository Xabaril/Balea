using System;
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
        private readonly BaleaConfiguration _configuration;
        private readonly BaleaOptions _options;

        public ConfigurationRuntimeAuthorizationServerStore(BaleaConfiguration configuration, BaleaOptions options)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public Task<AuthotizationContext> FindAuthorizationAsync(ClaimsPrincipal user)
        {
            var sourceRoleClaims = user.GetClaimValues(_options.SourceRoleClaimType);
            var application = _configuration.Applications.GetByName(_options.ApplicationName);
            var delegation = application.Delegations.GetCurrentDelegation(user.GetSubjectId(_options.SourceNameIdentifierClaimType));
            var subject = GetSubject(user, delegation);
            var roles = application.Roles
                    .Where(role => role.Subjects.Contains(subject, StringComparer.InvariantCultureIgnoreCase) || 
                                   role.Mappings.Any(m => sourceRoleClaims.Contains(m, StringComparer.InvariantCultureIgnoreCase)))
                    .Select(role => role.To());

            var authorization = new AuthotizationContext(roles, delegation.To());

            return Task.FromResult(authorization);
        }

        private string GetSubject(ClaimsPrincipal user, DelegationConfiguration delegation)
        {
            return delegation?.Who ?? user.GetSubjectId(_options.SourceNameIdentifierClaimType);
        }
    }
}
