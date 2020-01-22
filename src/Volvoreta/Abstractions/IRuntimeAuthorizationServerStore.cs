using System.Security.Claims;
using System.Threading.Tasks;
using Volvoreta.Model;

namespace Volvoreta.Abstractions
{
    public interface IRuntimeAuthorizationServerStore
    {
        Task<AuthotizationResult> FindAsync(ClaimsPrincipal user);
        Task<bool> IsInRoleAsync(ClaimsPrincipal user, string role);
        Task<bool> HasPermissionAsync(ClaimsPrincipal user, string permission);
    }
}