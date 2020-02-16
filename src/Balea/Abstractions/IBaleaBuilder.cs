using Microsoft.Extensions.DependencyInjection;

namespace Balea.Abstractions
{
    public interface IBaleaBuilder
    {
        IServiceCollection Services { get; }
    }

    internal sealed class BaleaBuilder : IBaleaBuilder
    {
        public BaleaBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }

        public IBaleaBuilder AddBalea()
        {
            Services.AddAuthorization();
            Services.AddHttpContextAccessor();

            return this;
        }
    }
}
