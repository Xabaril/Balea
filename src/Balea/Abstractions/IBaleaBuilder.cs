using Balea.DSL.PropertyBags;
using Microsoft.Extensions.DependencyInjection;

namespace Balea.Abstractions
{
    /// <summary>
    /// The builder used to register Balea and dependant services.
    /// </summary>
    public interface IBaleaBuilder
    {
        /// <summary>
        /// Gets the Microsoft.Extensions.DependencyInjection.IServiceCollection into which Balea services should be registered.
        /// </summary>
        IServiceCollection Services { get; }

        /// <summary>
        /// Allow to register a new <see cref="IPropertyBagBuilder"/> to be used on Balea DSL policies.
        /// </summary>
        /// <typeparam name="TPropertyBagBuilder">The <see cref="IPropertyBagBuilder"/> to be used.</typeparam>
        /// <returns>A new <see cref="IBaleaBuilder"/> that can be chained for register services.</returns>
        IBaleaBuilder AddDSLPropertyBagBuilder<TPropertyBagBuilder>() 
            where TPropertyBagBuilder : class, IPropertyBagBuilder;
    }

    internal sealed class BaleaBuilder : IBaleaBuilder
    {
        public BaleaBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }

        public IBaleaBuilder AddDSLPropertyBagBuilder<TPropertyBagBuilder>() where TPropertyBagBuilder : class, IPropertyBagBuilder
        {
            Services.AddScoped<IPropertyBagBuilder, TPropertyBagBuilder>();
            return this;
        }
    }
}
