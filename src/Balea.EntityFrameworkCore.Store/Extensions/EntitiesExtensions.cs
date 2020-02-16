using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Balea.Model;

namespace Balea.EntityFrameworkCore.Store.Entities
{
    public static class EntitiesExtensions
    {
        public static Role To(this RoleEntity role)
        {
            if (role is null)
            {
                return null;
            }

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
            if (delegation is null)
            {
                return null;
            }

            return new Delegation(
                    delegation.Who.Sub,
                    delegation.Whom.Sub,
                    delegation.From,
                    delegation.To
                );
        }

        public static Task<DelegationEntity> GetCurrentDelegation(this DbSet<DelegationEntity> delegations, string subjectId)
        {
            var now = DateTime.UtcNow;
            return delegations
                .Include(d => d.Who)
                .Include(d => d.Whom)
                .FirstOrDefaultAsync(d => 
                    d.Selected && 
                    d.From <= now && d.To >= now && 
                    d.Whom.Sub == subjectId);
        }

        public static Task<ApplicationEntity> GetByName(this DbSet<ApplicationEntity> applications, string name)
        {
            return applications.FirstOrDefaultAsync(a => a.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
