using System.Collections.Generic;

namespace Volvoreta.Model
{
    public class AuthotizationResult
    {
        public AuthotizationResult(IEnumerable<Role> roles, Delegation delegation)
        {
            Roles = roles;
            Delegation = delegation;
        }

        public IEnumerable<Role> Roles { get; set; }
        public Delegation Delegation { get; set; }
    }
}
