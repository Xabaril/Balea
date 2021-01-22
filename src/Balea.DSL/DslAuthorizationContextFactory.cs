using Balea.DSL.PropertyBags;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;

namespace Balea.DSL
{
    /// <summary>
    /// A factory for <see cref="DslAuthorizationContext"/>.
    /// </summary>
    public class DslAuthorizationContextFactory
    {
        private readonly IEnumerable<IPropertyBagBuilder> _propertyBags;

        public DslAuthorizationContextFactory(IEnumerable<IPropertyBagBuilder> propertyBags)
        {
            _propertyBags = propertyBags ?? throw new ArgumentNullException(nameof(propertyBags));
        }

        /// <summary>
        /// Create a new <see cref="DslAuthorizationContext"/> using registered <see cref="IPropertyBagBuilder"/> elements.
        /// </summary>
        /// <param name="authorizationHandlerContext">The ASP.NET Core authorization handler context used.</param>
        /// <returns>A new <see cref="DslAuthorizationContext"/> created.</returns>
        public DslAuthorizationContext Create(AuthorizationHandlerContext authorizationHandlerContext)
        {
            var context = new DslAuthorizationContext();

            foreach (var propertyBag in _propertyBags)
            {
                context.AddBag(
                    propertyBagName: propertyBag.BagName,
                    items: propertyBag.Build(authorizationHandlerContext));
            }

            return context;
        }
    }
}
