using System;
using Volvoreta.Abstractions;
using Volvoreta.EntityFrameworkCore.Store;
using Volvoreta.EntityFrameworkCore.Store.DbContexts;
using Volvoreta.EntityFrameworkCore.Store.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EntityFrameworkCoreStoreExtensions
    {
        public static  IVolvoretaBuilder AddEntityFrameworkCoreStore(this IVolvoretaBuilder builder, Action<StoreOptions> configurer = null)
        {
            var options = new StoreOptions();
            configurer?.Invoke(options);

            builder.Services.AddDbContext<StoreDbContext>(optionsAction => options.ConfigureDbContext?.Invoke(optionsAction));

            builder.Services.AddScoped<IRuntimeAuthorizationServerStore, EntityFrameworkCoreRuntimeAuthorizationServerStore>();

            return builder;
        }
    }
}
