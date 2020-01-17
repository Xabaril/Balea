using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Volvoreta.Abstractions;
using Volvoreta.Configuration.Store;
using Volvoreta.Configuration.Store.Model;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConfigurationStoreExtensions
    {
        private const string DefaultSectionName = "Volvoreta";

        public static IVolvoretaBuilder AddConfigurationStore(this IVolvoretaBuilder builder, IConfiguration configuration, string key = DefaultSectionName)
        {
            builder.Services.AddOptions();
            builder.Services.Configure<VolvoretaConfiguration>(configuration.GetSection(key));
            builder.Services.AddScoped(sp => sp.GetRequiredService<IOptionsSnapshot<VolvoretaConfiguration>>().Value);
            builder.Services.AddScoped<IRuntimeAuthorizationServerStore, ConfigurationRuntimeAuthorizationServerStore>();

            return builder;
        }
    }
}
