using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Balea.DSL
{
    /// <summary>
    /// A factory of <see cref="AbacAuthorizationContext"/> objects.
    /// </summary>
    public class AbacAuthorizationContextFactory
    {
        private readonly IEnumerable<IPropertyBag> _propertyBags;

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="propertyBags">The collection of property bags that can be used when the context is created.</param>
        public AbacAuthorizationContextFactory(IEnumerable<IPropertyBag> propertyBags)
        {
            _propertyBags = propertyBags ?? throw new ArgumentNullException(nameof(propertyBags));
        }

        /// <summary>
        /// Create a new <see cref="AbacAuthorizationContext"/> using registered <see cref="IPropertyBag"/> elements.
        /// </summary>
        /// <param name="authorizationHandlerContext">The ASP.NET Core authorization handler context used.</param>
        /// <returns>A new <see cref="AbacAuthorizationContext"/> created.</returns>
        public async Task<AbacAuthorizationContext> Create(AuthorizationHandlerContext authorizationHandlerContext)
        {
            var context = new AbacAuthorizationContext();

            foreach (var propertyBag in _propertyBags)
            {
                //initialize the property bag and add it to the collection of property bags to be used.
                await propertyBag.Initialize(authorizationHandlerContext);

                context.AddBag(propertyBag);
            }

            return context;
        }
    }
}
