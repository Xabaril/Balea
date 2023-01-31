using System.Collections.Generic;

namespace Balea.Model
{
    public class AuthorizationContext
    {
        public AuthorizationContext(IEnumerable<Role> roles, Delegation delegation)
        {
            Roles = roles;
            Delegation = delegation;
        }

        public IEnumerable<Role> Roles { get; set; }

        public Delegation Delegation { get; set; }
    }
}
