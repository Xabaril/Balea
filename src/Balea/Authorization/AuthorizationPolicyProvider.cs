using Balea.Authorization.Abac;
using Balea.Authorization.Rbac;
using Balea.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;

namespace Balea.Authorization
{
    public class AuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        private readonly AuthorizationOptions _options;
        private readonly BaleaOptions _baleaOptions;
        private readonly ILogger<AuthorizationPolicyProvider> _logger;

        public AuthorizationPolicyProvider(
            IOptions<AuthorizationOptions> options,
            BaleaOptions baleaOptions,
            ILogger<AuthorizationPolicyProvider> logger)
            : base(options)
        {
            _options = options.Value;
            _baleaOptions = baleaOptions;
            _logger = logger;
        }

        public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            var policy = await base.GetPolicyAsync(policyName);

            if (policy is null)
            {
                _logger.AuthorizationPolicyNotFound(policyName);

                //setup abac or rbac requirement
                var requirement = policyName.Contains("abac__")
                    ? (IAuthorizationRequirement) new AbacRequirement(policyName)
                    :  new PermissionRequirement(policyName);

                if (_baleaOptions.Schemes.Any())
                {
                    policy = new AuthorizationPolicyBuilder()
                        .AddRequirements(requirement)
                        .AddAuthenticationSchemes(_baleaOptions.Schemes.ToArray())
                        .Build();

                    _logger.CreatingAuthorizationPolicy(policyName, _baleaOptions.Schemes);
                }
                else
                {
                    policy = new AuthorizationPolicyBuilder()
                        .AddRequirements(requirement)
                        .Build();

                    _logger.CreatingAuthorizationPolicy(policyName);
                }

                // By default, policies are stored in the AuthorizationOptions instance (singleton),
                // so we can cache all the policies created at runtime there to create the policies only once
                _options.AddPolicy(policyName, policy);
            }
            else
            {
                _logger.AuthorizationPolicyFound(policyName);
            }

            return policy;
        }
    }
}
