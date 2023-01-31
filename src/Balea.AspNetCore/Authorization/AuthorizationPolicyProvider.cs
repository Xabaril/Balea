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
		private readonly BaleaWebHost _webHost;
        private readonly ILogger<AuthorizationPolicyProvider> _logger;

        private object sync_root = new object();

        public AuthorizationPolicyProvider(
            IOptions<AuthorizationOptions> options,
			BaleaWebHost webHost,
            ILogger<AuthorizationPolicyProvider> logger)
            : base(options)
        {
            _options = options.Value;
			_webHost = webHost;
            _logger = logger;
        }

        public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            var policy = await base.GetPolicyAsync(policyName);

            if (policy is null)
            {
                _logger.AuthorizationPolicyNotFound(policyName);

                //setup abac or rbac requirement
                var abacPrefix = new AbacPrefix(policyName);
                var requirement = policyName.Equals(abacPrefix.ToString())
                    ? (IAuthorizationRequirement) new AbacRequirement(abacPrefix.Policy)
                    :  new PermissionRequirement(policyName);

                if (_webHost.Schemes.Any())
                {
                    policy = new AuthorizationPolicyBuilder()
                        .AddRequirements(requirement)
                        .AddAuthenticationSchemes(_webHost.Schemes.ToArray())
                        .Build();

                    _logger.CreatingAuthorizationPolicy(policyName, _webHost.Schemes);
                }
                else
                {
                    policy = new AuthorizationPolicyBuilder()
                        .AddRequirements(requirement)
                        .Build();

                    _logger.CreatingAuthorizationPolicy(policyName);
                }

                lock (sync_root)
                {
                    // By default, policies are stored in the AuthorizationOptions instance (singleton),
                    // so we can cache all the policies created at runtime there to create the policies only once
                    // the internal dictionary is a plain dictionary ( not concurrent ), we need to ensure
                    // a thread safe access 
                    _options.AddPolicy(policyName, policy);
                }
            }
            else
            {
                _logger.AuthorizationPolicyFound(policyName);
            }

            return policy;
        }
    }
}
