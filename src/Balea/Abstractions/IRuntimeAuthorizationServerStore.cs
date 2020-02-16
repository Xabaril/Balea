using System.Security.Claims;
using System.Threading.Tasks;
using Balea.Model;

namespace Balea.Abstractions
{
    public interface IRuntimeAuthorizationServerStore
    {
        Task<AuthotizationResult> FindAuthorizationAsync(ClaimsPrincipal user);
        Task<bool> IsInRoleAsync(ClaimsPrincipal user, string role);
        Task<bool> HasPermissionAsync(ClaimsPrincipal user, string permission);
    }
}