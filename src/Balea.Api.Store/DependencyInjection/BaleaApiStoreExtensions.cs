using Balea.Abstractions;
using Balea.Api.Store;
using Balea.Api.Store.DelegatingHandlers;
using Balea.Api.Store.Options;
using Microsoft.Extensions.Options;
using Polly;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class BaleaApiStoreExtensions
    {
        private const string API_KEY_HEADER = "X-Api-Key";

        public static IBaleaBuilder AddClient(this IBaleaBuilder builder, Action<StoreOptions> setup)
        {
            var options = new StoreOptions();
            setup?.Invoke(options);

            builder.Services.Configure<StoreOptions>(setup =>
            {
                setup.CacheEnabled = options.CacheEnabled;
                setup.AbsoluteExpirationRelativeToNow = options.AbsoluteExpirationRelativeToNow;
                setup.SlidingExpiration = options.SlidingExpiration;
                setup.RetryCount = options.RetryCount;
                setup.HandledEventsAllowedBeforeBreaking = options.HandledEventsAllowedBeforeBreaking;
                setup.DurationOfBreak = options.DurationOfBreak;
                setup.BaseAddress = options.BaseAddress;
                setup.ApiKey = options.ApiKey;
            });

            builder.Services.AddScoped(sp => sp.GetRequiredService<IOptions<StoreOptions>>().Value);

            builder.Services.AddTransient<LoggingHandler>();

            builder.Services
                .AddHttpClient(Constants.BaleaClient, (sp, client) =>
                {
                    var options = sp.GetRequiredService<IOptions<StoreOptions>>().Value;
                    client.BaseAddress = options.BaseAddress;
                    client.DefaultRequestHeaders.Add(API_KEY_HEADER, options.ApiKey);
                })
                .AddHttpMessageHandler<LoggingHandler>()
                .AddTransientHttpErrorPolicy(p => p.RetryAsync(options.RetryCount))
                .AddTransientHttpErrorPolicy(p =>
                    p.CircuitBreakerAsync(options.HandledEventsAllowedBeforeBreaking, TimeSpan.FromSeconds(options.DurationOfBreak)));

            builder.Services.AddScoped<IRuntimeAuthorizationServerStore, ApiRuntimeAuthorizationServerStore>();

            return builder;
        }
    }
}
