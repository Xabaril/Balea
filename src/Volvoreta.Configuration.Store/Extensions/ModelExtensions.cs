using Volvoreta.Model;

namespace Volvoreta.Configuration.Store.Model
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
    }
}
