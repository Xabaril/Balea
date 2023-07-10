using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Balea.Authorization.Abac.Context
{
    public interface IAspNetPropertyBag : IPropertyBag
	{
        /// <summary>
        /// Initialize the property bag.
        /// </summary>
        /// <param name="authorizationHandlerContext">The authorization handler context to use.</param>
        /// <returns>A Task that complete when service finished.</returns>
        Task Initialize(AuthorizationHandlerContext authorizationHandlerContext);
    }
}
