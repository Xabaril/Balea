using System.Collections.Generic;

namespace Balea.Authorization.Abac.Context
{
    /// <summary>
    /// Represent the collection of property bag's to use on DSL evaluation process. Balea
    /// use pre-defined property bags builders ( endpoint, user etc ) to create this context and
    /// use it on rules.
    /// </summary>
    public class AbacAuthorizationContext
    {
        private readonly Dictionary<string, IPropertyBag> _propertyBagsHolder = new();

        /// <summary>
        /// Get the property bag by name.
        /// </summary>
        /// <param name="propertyBagName">The name of the property bag.</param>
        /// <returns>A dictionary that represent the items on the property bag specified by <paramref name="propertyBagName"/></returns>
        public IPropertyBag this[string propertyBagName]
        {
            get
            {
                return _propertyBagsHolder[propertyBagName];
            }
            internal set
            {
                _propertyBagsHolder.Add(propertyBagName, value);
            }
        }

        public AbacAuthorizationContext() { }

        public void AddBag(IPropertyBag propertyBag)
        {
            _propertyBagsHolder.TryAdd(propertyBag.Name, propertyBag);
        }
    }
}
