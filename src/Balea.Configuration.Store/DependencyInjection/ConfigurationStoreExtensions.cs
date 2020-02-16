using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Balea.Abstractions;
using Balea.Configuration.Store;
using Balea.Configuration.Store.Model;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConfigurationStoreExtensions
    {
        private const string DefaultSectionName = "Balea";

        public static IBaleaBuilder AddConfigurationStore(this IBaleaBuilder builder, IConfiguration configuration, string key = DefaultSectionName)
        {
            builder.Services.AddOptions();
            builder.Services.Configure<BaleaConfiguration>(configuration.GetSection(key));
            builder.Services.AddScoped(sp => sp.GetRequiredService<IOptionsSnapshot<BaleaConfiguration>>().Value);
            builder.Services.AddScoped<IRuntimeAuthorizationServerStore, ConfigurationRuntimeAuthorizationServerStore>();

            return builder;
        }
    }
}
