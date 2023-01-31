using System.Security.Claims;
using System.Threading.Tasks;

namespace Balea.Abstractions
{
    public interface IPermissionEvaluator
    {
        Task<bool> HasPermissionAsync(ClaimsPrincipal user, string permission);
    }
}
