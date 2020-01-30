using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using Volvoreta;
using Volvoreta.Abstractions;
using Volvoreta.Authorization;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IVolvoretaBuilder AddVolvoreta(this IServiceCollection services, Action<VolvoretaOptions> setup)
        {
            _ = services ?? throw new ArgumentNullException(nameof(services));
            _ = setup ?? throw new ArgumentNullException(nameof(setup));

            services.Configure(setup);

            return services.AddVolvoreta();
        }

        public static IVolvoretaBuilder AddVolvoreta(this IServiceCollection services)
        {
            _ = services ?? throw new ArgumentNullException(nameof(services));

            services.AddHttpContextAccessor();
            services.AddScoped(sp => sp.GetRequiredService<IOptions<VolvoretaOptions>>().Value);
            services.AddTransient<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
            services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();

            return new VolvoretaBuilder(services);
        }
    }
}
