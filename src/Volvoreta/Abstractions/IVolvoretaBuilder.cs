using Microsoft.Extensions.DependencyInjection;

namespace Volvoreta.Abstractions
{
    public interface IVolvoretaBuilder
    {
        IServiceCollection Services { get; }
    }

    internal sealed class VolvoretaBuilder : IVolvoretaBuilder
    {
        public VolvoretaBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }

        public IVolvoretaBuilder AddVolvoreta()
        {
            Services.AddAuthorization();
            Services.AddHttpContextAccessor();

            return this;
        }
    }
}
