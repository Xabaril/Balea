using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Balea.Abstractions;
using Balea.EntityFrameworkCore.Store.DbContexts;
using Balea.EntityFrameworkCore.Store.Entities;
using Balea.Model;

namespace Balea.EntityFrameworkCore.Store
{
    public class EntityFrameworkCoreRuntimeAuthorizationServerStore : IRuntimeAuthorizationServerStore
    {
        private readonly StoreDbContext _context;
        private readonly BaleaOptions _options;

        public EntityFrameworkCoreRuntimeAuthorizationServerStore(StoreDbContext context, BaleaOptions options)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task<AuthotizationContext> FindAuthorizationAsync(ClaimsPrincipal user)
        {
            var sourceRoleClaims = user.GetClaimValues(_options.SourceRoleClaimType);
            var delegation = await _context.Delegations.GetCurrentDelegation(user.GetSubjectId(_options.SourceNameIdentifierClaimType));
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
                        role.Application.Name == _options.ApplicationName &&
                        role.Enabled &&
                        (role.Subjects.Any(rs => rs.Subject.Sub == subject) || role.Mappings.Any(rm => sourceRoleClaims.Contains(rm.Mapping.Name)))
                    )
                    .ToListAsync();

            return new AuthotizationContext(roles.Select(r => r.To()), delegation.To());
        }

        private string GetSubject(ClaimsPrincipal user, DelegationEntity delegation)
        {
            return delegation?.Who?.Sub ?? user.GetSubjectId(_options.SourceNameIdentifierClaimType);
        }
    }
}
