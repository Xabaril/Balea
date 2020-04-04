using Balea.Model;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Balea.Abstractions
{
    public interface IRuntimeAuthorizationServerStore
    {
        Task<AuthotizationContext> FindAuthorizationAsync(ClaimsPrincipal user, CancellationToken cancellationToken = default);
    }
}