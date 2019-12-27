using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Volvoreta.Model;

namespace Volvoreta.Abstractions
{
    public interface IRuntimeAuthorizationServerStore
    {
        Task<IEnumerable<Role>> FindRolesAsync(ClaimsPrincipal user);
        Task<Role> FindRoleAsync(ClaimsPrincipal user);
        Task<Role> AddRoleAsync(ClaimsPrincipal user);
        Task<Role> UpdateRoleAsync(ClaimsPrincipal user);
        Task RemoveRoleAsync(ClaimsPrincipal user);
        Task<bool> IsInRoleAsync(ClaimsPrincipal user, string role);
        Task<bool> HasPermissionAsync(ClaimsPrincipal user, string permission);
    }
}