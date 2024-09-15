using Balea.Authorization.Abac.Context;
using Microsoft.Extensions.DependencyInjection;

namespace Balea.Abstractions
{
    internal sealed class BaleaBuilder : IBaleaBuilder
    {
        public BaleaBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }

        public IBaleaBuilder AddPropertyBag<TPropertyBag>() where TPropertyBag: class, IPropertyBag
        {
            Services.AddScoped<IPropertyBag, TPropertyBag>();
            return this;
        }
    }
}
