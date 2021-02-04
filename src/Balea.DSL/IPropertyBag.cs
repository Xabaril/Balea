using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Balea.DSL
{
    public interface IPropertyBag
    {
        /// <summary>
        /// Get the name of the property bag.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Initialize the property bag.
        /// </summary>
        /// <param name="authorizationHandlerContext">The authorization handler context to use.</param>
        /// <returns>A Task that complete when service finished.</returns>
        Task Initialize(AuthorizationHandlerContext authorizationHandlerContext);

        /// <summary>
        /// The property bag indexer.
        /// </summary>
        /// <param name="propertyName">The property name to get.</param>
        /// <returns>The <paramref name="propertyName"/> property value.</returns>
        object this[string propertyName] { get; }
    }
}
