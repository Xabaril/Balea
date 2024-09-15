using Balea.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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

        public static Task<DelegationEntity> GetCurrentDelegation(
            this DbSet<DelegationEntity> delegations,
            string subjectId,
            string applicationName,
            CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;
            return delegations
                .Include(d => d.Who)
                .Include(d => d.Whom)
                .Include(d => d.Application)
                .FirstOrDefaultAsync(
                    d =>
                        d.Selected &&
                        d.From <= now && d.To >= now &&
                        d.Whom.Sub == subjectId &&
                        d.Application.Name == applicationName,
                    cancellationToken);
        }

        public static Task<Delegation> GetDelegation(
            this DbSet<DelegationEntity> delegations,
            string subjectId,
            string applicationName,
            CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;
            return delegations
                .AsNoTracking()
                .Where(
                    d =>
                        d.Selected &&
                        d.From <= now && d.To >= now &&
                        d.Whom.Sub == subjectId &&
                        d.Application.Name == applicationName)
                .Select(
                    d => new Delegation(
                        d.Who.Sub,
                        d.Whom.Sub,
                        d.From,
                        d.To))
                .FirstOrDefaultAsync(cancellationToken);
        }

        public static Policy To(this PolicyEntity policy)
        {
            if (policy is null)
            {
                return null;
            }

            return new Policy(policy.Name, policy.Content);
        }
    }
}
