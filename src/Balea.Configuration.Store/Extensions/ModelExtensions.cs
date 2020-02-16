using System;
using System.Linq;
using Balea.Model;

namespace Balea.Configuration.Store.Model
{
    public static class ModelExtensions
    {
        public static Role To(this RoleConfiguration role)
        {
            if (role is null)
            {
                return null;
            }

            return new Role(
                    role.Name,
                    role.Description,
                    role.Subjects,
                    role.Mappings,
                    role.Permissions,
                    role.Enabled
                );
        }

        public static Delegation To(this DelegationConfiguration delegation)
        {
            if (delegation is null)
            {
                return null;
            }

            return new Delegation(
                    delegation.Who,
                    delegation.Whom,
                    delegation.From,
                    delegation.To
                );
        }

        public static DelegationConfiguration GetCurrentDelegation(this DelegationConfiguration[] delegations, string subjectId)
        {
            return delegations.FirstOrDefault(d => d.Active && d.Whom == subjectId);
        }

        public static ApplicationConfiguration GetByName(this ApplicationConfiguration [] applications, string name)
        {
            return applications.First(a => a.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
