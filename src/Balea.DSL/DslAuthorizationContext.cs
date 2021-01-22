using System.Collections.Generic;

namespace Balea.DSL
{
    /// <summary>
    /// Represent the collection of property bag's to use on DSL evaluation process. Balea
    /// use pre-defined property bags builders ( endpoint, user etc ) to create this context and
    /// use it on rules.
    /// </summary>
    public class DslAuthorizationContext
    {
        private Dictionary<string, Dictionary<string, object>> _propertyBagHolder = new();

        /// <summary>
        /// Get the property bag by name.
        /// </summary>
        /// <param name="propertyBagName">The name of the property bag.</param>
        /// <returns>A dictionary that represent the items on the property bag specified by <paramref name="propertyBagName"/></returns>
        public Dictionary<string, object> this[string propertyBagName]
        {
            get
            {
                return _propertyBagHolder[propertyBagName];
            }
            internal set
            {
                _propertyBagHolder.Add(propertyBagName, value);
            }
        }

        internal DslAuthorizationContext() { }

        internal void AddBag(string propertyBagName, Dictionary<string, object> items)
        {
            if (_propertyBagHolder.ContainsKey(propertyBagName))
            {
                //replace or add new entries to existing property bag 
                foreach (var item in items)
                {
                    if (!_propertyBagHolder[propertyBagName].ContainsKey(item.Key))
                    {
                        _propertyBagHolder[propertyBagName].Add(item.Key, item.Value);
                    }
                    else
                    {
                        _propertyBagHolder[propertyBagName][item.Key] = item.Value;
                    }
                }
            }
            else
            {
                //add new property bag
                _propertyBagHolder.Add(propertyBagName, items);
            }
        }
    }
}
