using System.Security.Claims;
using System.Threading.Tasks;
using Balea.Model;

namespace Balea.Abstractions
{
    public interface IRuntimeAuthorizationServerStore
    {
        Task<AuthotizationContext> FindAuthorizationAsync(ClaimsPrincipal user);
    }
}