using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Volvoreta.Abstractions;
using Volvoreta.EntityFrameworkCore.Store.DbContexts;
using Volvoreta.EntityFrameworkCore.Store.Entities;
using Volvoreta.Model;

namespace Volvoreta.EntityFrameworkCore.Store
{
    public class EntityFrameworkCoreRuntimeAuthorizationServerStore : IRuntimeAuthorizationServerStore
    {
        private readonly StoreDbContext _context;

        public EntityFrameworkCoreRuntimeAuthorizationServerStore(StoreDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<AuthotizationResult> FindAsync(ClaimsPrincipal user)
        {
            var claimRoles = user.FindAll(ClaimTypes.Role).Select(x => x.Value);
            var delegation = await _context.Delegations.FirstOrDefaultAsync(d => d.Active); ;
            var subject = delegation?.Who ?? user.GetSubjectId();
            var roles = await _context.Roles
                    .Include(r => r.Mappings)
                    .ThenInclude(rm => rm.Mapping)
                    .Include(r => r.Subjects)
                    .ThenInclude(rs => rs.Subject)
                    .Include(r => r.Permissions)
                    .ThenInclude(rp => rp.Permission)
                    .Where(role =>
                        role.Enabled &&
                        (role.Subjects.Any(rs => rs.Subject.Sub == subject) || role.Mappings.Any(rm => claimRoles.Contains(rm.Mapping.Name)))
                    )
                    .ToListAsync();

            return new AuthotizationResult(roles.Select(r => r.To()), delegation.To());
        }

        public Task<bool> HasPermissionAsync(ClaimsPrincipal user, string permission)
        {
            var subject = user.GetSubjectId();
            var roles = user.FindAll(ClaimTypes.Role).Select(x => x.Value);

            return 
                _context.Roles
                    .Include(r => r.Permissions)
                    .ThenInclude(rp => rp.Permission)
                    .Where(role =>
                        role.Enabled &&
                        (role.Subjects.Any(rs => rs.Subject.Sub == subject) || role.Mappings.Any(rm => roles.Contains(rm.Mapping.Name)))
                    )
                    .SelectMany(role => role.Permissions)
                    .AnyAsync(rp => rp.Permission.Name == permission);
        }

        public Task<bool> IsInRoleAsync(ClaimsPrincipal user, string role)
        {
            var subject = user.GetSubjectId();
            var roles = user.FindAll(ClaimTypes.Role).Select(x => x.Value);

            return
                _context.Roles
                    .AnyAsync(r =>
                        r.Enabled &&
                        r.Name == role &&
                        (r.Subjects.Any(rs => rs.Subject.Sub == subject) || r.Mappings.Any(rm => roles.Contains(rm.Mapping.Name)))
                    );
        }
    }
}
