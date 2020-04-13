using Balea.Model;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Balea.Abstractions
{
    public interface IRuntimeAuthorizationServerStore
    {
        Task<AuthorizationContext> FindAuthorizationAsync(ClaimsPrincipal user, CancellationToken cancellationToken = default);
    }
}