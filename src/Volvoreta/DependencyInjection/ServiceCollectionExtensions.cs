using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using Volvoreta.Abstractions;
using Volvoreta.Authorization;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IVolvoretaBuilder AddVolvoreta(this IServiceCollection services, Action<VolvoretaOptions> configureOptions = null)
        {
            var options = new VolvoretaOptions();
            configureOptions?.Invoke(options); 

            services.Configure<VolvoretaOptions>(opt =>
            {
                opt.DefaultRoleClaimType = options.DefaultRoleClaimType;
                opt.DefaultApplicationName = options.DefaultApplicationName;
            });
            services.AddHttpContextAccessor();
            services.AddScoped(sp => sp.GetRequiredService<IOptions<VolvoretaOptions>>().Value);
            services.AddTransient<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
            services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();
            return new VolvoretaBuilder(services);
        }
    }
}
