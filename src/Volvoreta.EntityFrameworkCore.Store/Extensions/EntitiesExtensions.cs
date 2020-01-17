using System.Linq;
using Volvoreta.Model;

namespace Volvoreta.EntityFrameworkCore.Store.Entities
{
    public static class EntitiesExtensions
    {
        public static Role To(this RoleEntity role)
        {
            return new Role(
                        role.Name,
                        role.Description,
                        role.Subjects.Select(rs => rs.Subject.Sub),
                        role.Mappings.Select(rm => rm.Mapping.Name),
                        role.Permissions.Select(rp => rp.Permission.Name),
                        role.Enabled
                    );
        }

        public static Delegation To(this DelegationEntity delegation)
        {
            return new Delegation(
                    delegation.Who,
                    delegation.Whom,
                    delegation.From,
                    delegation.To
                );
        }
    }
}
