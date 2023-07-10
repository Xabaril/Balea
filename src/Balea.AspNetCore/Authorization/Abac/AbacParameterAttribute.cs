using System;

namespace Balea.Authorization.Abac
{
    /// <summary>
    /// Decorate action parameter to be allow be used on auhtorization policies using the
    /// <see cref="ParameterPropertyBag"/> property bag.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public class AbacParameterAttribute
        : Attribute
    {
        /// <summary>
        /// Modify the name of the parameter to be used on ABAC policies.
        /// </summary>
        public string Name { get; set; }
    }
}
