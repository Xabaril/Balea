using Microsoft.AspNetCore.Authorization;

namespace Balea.Authorization
{
    //
    // Summary:
    //     Specifies that the class or method that this attribute is applied to requires
    //     the specified authorization using Balea DSL.
    public class AbacPolicyAttribute
        : AuthorizeAttribute
    {
        /// <summary>
        /// Initialize a new instance.
        /// </summary>
        /// <param name="policyName">The ABAC policy name registered on Balea to be used.</param>
        public AbacPolicyAttribute(string policyName) :
            base($"abac__{policyName}")
        { }
    }
}
