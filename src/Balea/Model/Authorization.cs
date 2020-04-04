using System.Collections.Generic;

namespace Balea.Model
{
    public class Authorization
    {
        public IEnumerable<Role> Roles { get; set; }
        public IEnumerable<Delegation> Delegations { get; set; }
    }
}
