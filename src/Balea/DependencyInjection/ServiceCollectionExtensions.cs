using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using Balea;
using Balea.Abstractions;
using Balea.Authorization;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IBaleaBuilder AddBalea(this IServiceCollection services, Action<BaleaOptions> setup)
        {
            _ = services ?? throw new ArgumentNullException(nameof(services));
            _ = setup ?? throw new ArgumentNullException(nameof(setup));

            services.Configure(setup);

            return services.AddBalea();
        }

        public static IBaleaBuilder AddBalea(this IServiceCollection services)
        {
            _ = services ?? throw new ArgumentNullException(nameof(services));

            services.AddAuthorization();
            services.AddHttpContextAccessor();
            services.AddSingleton(sp => sp.GetRequiredService<IOptions<BaleaOptions>>().Value);
            services.AddScoped(sp => sp.GetRequiredService<IOptions<BaleaOptions>>().Value);
            services.AddScoped<IPermissionEvaluator, DefaultPermissionEvaluator>();
            services.AddTransient<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
            services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();

            return new BaleaBuilder(services);
        }
    }
}
