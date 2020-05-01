using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Balea.Authorization
{
    public class AuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        private readonly BaleaOptions _baleaOptions;
        private readonly AuthorizationOptions _options;

        public AuthorizationPolicyProvider(
            IOptions<AuthorizationOptions> options,
            BaleaOptions baleaOptions)
            : base(options)
        {
            _baleaOptions = baleaOptions;
            _options = options.Value;
        }

        public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            var policy = await base.GetPolicyAsync(policyName);

            if (policy is null)
            {
                if (_baleaOptions.Schemes.Any())
                {
                    policy = new AuthorizationPolicyBuilder()
                        .AddRequirements(new PermissionRequirement(policyName))
                        .AddAuthenticationSchemes(_baleaOptions.Schemes.ToArray())
                        .Build();
                }
                else
                {
                    policy = new AuthorizationPolicyBuilder()
                        .AddRequirements(new PermissionRequirement(policyName))
                        .Build();
                }

                // By default, policies are stored in the AuthorizationOptions instance (singleton),
                // so we can cache all the policies created at runtime there to create the policies only once
                _options.AddPolicy(policyName, policy);
            }

            return policy;
        }
    }
}
