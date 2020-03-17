using Balea.Abstractions;
using Balea.EntityFrameworkCore.Store;
using Balea.EntityFrameworkCore.Store.DbContexts;
using Balea.EntityFrameworkCore.Store.Options;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EntityFrameworkCoreStoreExtensions
    {
        public static IBaleaBuilder AddEntityFrameworkCoreStore(this IBaleaBuilder builder, Action<StoreOptions> configurer = null)
        {
            var options = new StoreOptions();
            configurer?.Invoke(options);

            if (options.ConfigureDbContext != null)
            {
                builder.Services.AddDbContext<BaleaDbContext>(optionsAction => options.ConfigureDbContext?.Invoke(optionsAction));
            }
            
            builder.Services.AddScoped<IRuntimeAuthorizationServerStore, EntityFrameworkCoreRuntimeAuthorizationServerStore>();

            return builder;
        }
    }
}
