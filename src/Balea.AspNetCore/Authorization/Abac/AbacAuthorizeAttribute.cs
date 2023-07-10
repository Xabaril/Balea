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
        /// <param name="policy">The ABAC policy  registered on Balea to be used.</param>
        public AbacAuthorizeAttribute(string policy) :
            base(new AbacPrefix(policy).ToString())
        { }
    }
}
