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
        private readonly VolvoretaOptions _options;

        public EntityFrameworkCoreRuntimeAuthorizationServerStore(StoreDbContext context, VolvoretaOptions options)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task<AuthotizationResult> FindAuthorizationAsync(ClaimsPrincipal user)
        {
            var claimRoles = user.GetClaimRoleValues();
            var delegation = await _context.Delegations.GetCurrentDelegation(user.GetSubjectId());
            var subject = GetSubject(user, delegation);
            var roles = await _context.Roles
                    .AsNoTracking()
                    .Include(r => r.Application)
                    .Include(r => r.Mappings)
                    .ThenInclude(rm => rm.Mapping)
                    .Include(r => r.Subjects)
                    .ThenInclude(rs => rs.Subject)
                    .Include(r => r.Permissions)
                    .ThenInclude(rp => rp.Permission)
                    .Where(role =>
                        role.Application.Name == _options.DefaultApplicationName &&
                        role.Enabled &&
                        (role.Subjects.Any(rs => rs.Subject.Sub == subject) || role.Mappings.Any(rm => claimRoles.Contains(rm.Mapping.Name)))
                    )
                    .ToListAsync();

            return new AuthotizationResult(roles.Select(r => r.To()), delegation.To());
        }

        public async Task<bool> HasPermissionAsync(ClaimsPrincipal user, string permission)
        {
            var claimRoles = user.GetClaimRoleValues();
            var delegation = await _context.Delegations.GetCurrentDelegation(user.GetSubjectId());
            var subject = GetSubject(user, delegation);

            return await
                _context.Roles
                    .AsNoTracking()
                    .Include(r => r.Application)
                    .Include(r => r.Permissions)
                    .ThenInclude(rp => rp.Permission)
                    .Where(role =>
                        role.Application.Name == _options.DefaultApplicationName &&
                        role.Enabled &&
                        (role.Subjects.Any(rs => rs.Subject.Sub == subject) || role.Mappings.Any(rm => claimRoles.Contains(rm.Mapping.Name)))
                    )
                    .SelectMany(role => role.Permissions)
                    .AnyAsync(rp => rp.Permission.Name == permission);
        }

        public async Task<bool> IsInRoleAsync(ClaimsPrincipal user, string role)
        {
            var claimRoles = user.GetClaimRoleValues();
            var delegation = await _context.Delegations.GetCurrentDelegation(user.GetSubjectId());
            var subject = GetSubject(user, delegation);

            return await
                _context.Roles
                    .AsNoTracking()
                    .AnyAsync(r =>
                        r.Enabled &&
                        r.Name == role &&
                        (r.Subjects.Any(rs => rs.Subject.Sub == subject) || r.Mappings.Any(rm => claimRoles.Contains(rm.Mapping.Name)))
                    );
        }

        private string GetSubject(ClaimsPrincipal user, DelegationEntity delegation)
        {
            return delegation?.Who.Sub ?? user.GetSubjectId();
        }
    }
}
