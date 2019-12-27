using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Volvoreta.Abstractions;
using Volvoreta.Configuration.Store.Model;
using Volvoreta.Model;

namespace Volvoreta.Configuration.Store
{
    public class ConfigurationRuntimePolicyServerStore : IRuntimeAuthorizationServerStore
    {
        private readonly VolvoretaConfiguration _volvoreta;

        public ConfigurationRuntimePolicyServerStore(VolvoretaConfiguration volvoreta)
        {
            _volvoreta = volvoreta;
        }

        public Task<Role> AddRoleAsync(ClaimsPrincipal user)
        {
            throw new System.NotImplementedException();
        }

        public Task<Role> FindRoleAsync(ClaimsPrincipal user)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Role>> FindRolesAsync(ClaimsPrincipal user)
        {
            var subject = user.GetSubjectId();
            var roles = user.FindAll(ClaimTypes.Role).Select(x => x.Value);
            
            return Task.FromResult(
                _volvoreta.Roles
                    .Where(role => role.Subjects.Contains(subject) || role.Mappings.Any(m => roles.Contains(m)))
                    .Select(role => new Role(
                        role.Name,
                        role.Description,
                        role.Subjects,
                        role.Mappings,
                        role.Permissions,
                        role.Enabled)
                    )
                );
        }

        public Task<bool> HasPermissionAsync(ClaimsPrincipal user, string permission)
        {
            var subject = user.GetSubjectId();
            var roles = user.FindAll(ClaimTypes.Role).Select(x => x.Value);

            return Task.FromResult(
                _volvoreta.Roles
                    .Where(role => 
                        role.Enabled &&
                        (role.Subjects.Contains(subject) || role.Mappings.Any(m => roles.Contains(m)))
                    )
                    .SelectMany(role => role.Permissions)
                    .Contains(permission)
            );
        }

        public Task<bool> IsInRoleAsync(ClaimsPrincipal user, string role)
        {
            return Task.FromResult(
                _volvoreta.Roles
                    .Any(r => r.Enabled && r.Name.Equals(role, StringComparison.InvariantCultureIgnoreCase))
            );
        }

        public Task RemoveRoleAsync(ClaimsPrincipal user)
        {
            throw new System.NotImplementedException();
        }

        public Task<Role> UpdateRoleAsync(ClaimsPrincipal user)
        {
            throw new System.NotImplementedException();
        }
    }
}
