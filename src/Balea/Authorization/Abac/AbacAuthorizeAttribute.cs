using Microsoft.AspNetCore.Authorization;

namespace Balea.Authorization.Abac
{
    //
    // Summary:
    //     Specifies that the class or method that this attribute is applied to requires
    //     the specified authorization using Balea DSL.
    public class AbacAuthorizeAttribute
        : AuthorizeAttribute
    {
        /// <summary>
        /// Initialize a new instance.
        /// </summary>
        /// <param name="policyName">The ABAC policy name registered on Balea to be used.</param>
        public AbacAuthorizeAttribute(string policyName) :
            base(new AbacPrefix(policyName).ToString())
        { }
    }
}
