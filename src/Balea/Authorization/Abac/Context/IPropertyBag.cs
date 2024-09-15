namespace Balea.Authorization.Abac.Context
{
    public interface IPropertyBag
    {
        /// <summary>
        /// Get the name of the property bag.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The property bag indexer to get single values of property
        /// </summary>
        /// <param name="propertyName">The property name to get.</param>
        /// <returns>The <paramref name="propertyName"/> property value.</returns>
        object this[string propertyName] { get; }

        /// <summary>
        /// Allow to check if a multiple property value contains some entry.
        /// </summary>
        /// <param name="propertyName">The property name to check.</param>
        /// <param name="value">The value to check.</param>
        /// <returns>True if the property bag contains <paramref name="value"/> for property <paramref name="propertyName"/>.</returns>
        bool Contains(string propertyName, object value);
    }
}
