using Balea.Abstractions;
using Balea.EntityFrameworkCore.Store.DbContexts;
using Balea.EntityFrameworkCore.Store.Entities;
using Balea.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Balea.EntityFrameworkCore.Store
{
    public class EntityFrameworkCoreRuntimeAuthorizationServerStore<TContext>
        : IRuntimeAuthorizationServerStore
        where TContext : BaleaDbContext
    {
        private readonly TContext _context;
        private readonly BaleaOptions _options;

        public EntityFrameworkCoreRuntimeAuthorizationServerStore(TContext context, BaleaOptions options)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task<AuthorizationContext> FindAuthorizationAsync(ClaimsPrincipal user, CancellationToken cancellationToken = default)
        {
            var userSubjectId = user.GetSubjectId(_options);
            var sourceRoleClaims = user.GetClaimValues(_options.DefaultClaimTypeMap.RoleClaimType);

            var delegation = await _context.Delegations.GetDelegation(
                userSubjectId,
                _options.ApplicationName,
                cancellationToken);

            var subject = delegation?.Who ?? userSubjectId;

            var roles = await _context.Roles
                .AsNoTracking()
                .Where(role =>
                    role.Application.Name == _options.ApplicationName &&
                    role.Enabled &&
                    (
                        role.Subjects.Any(rs => rs.Subject.Sub == subject) ||
                        role.Mappings.Any(rm => sourceRoleClaims.Contains(rm.Mapping.Name))
                    )
                )
                .Select(role => new Role(
                    role.Name,
                    role.Description,
                    role.Subjects.Select(rs => rs.Subject.Sub),
                    role.Mappings.Select(rm => rm.Mapping.Name),
                    role.Permissions.Select(rp => rp.Permission.Name),
                    role.Enabled
                ))
                .ToListAsync(cancellationToken);

            return new AuthorizationContext(roles, delegation);
        }

        public async Task<Policy> GetPolicyAsync(string name, CancellationToken cancellationToken = default)
        {
            var policy = await _context
                .Policies
                .AsNoTracking()
                .Where(p => p.Application.Name == _options.ApplicationName && p.Name == name)
                .Select(p => new Policy(p.Name, p.Content))
                .FirstOrDefaultAsync(cancellationToken);

            return policy;
        }
    }
}
