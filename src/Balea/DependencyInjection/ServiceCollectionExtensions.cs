using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using Balea;
using Balea.Abstractions;
using Balea.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Balea.DSL.PropertyBags;
using Balea.DSL;

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

            //add balea required services
            services.AddAuthorization();
            services.AddHttpContextAccessor();
            services.AddSingleton(sp => sp.GetRequiredService<IOptions<BaleaOptions>>().Value);
            services.AddScoped<IPermissionEvaluator, DefaultPermissionEvaluator>();
            services.AddTransient<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
            services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();
            services.AddTransient<IPolicyEvaluator, BaleaPolicyEvaluator>();

            //add balea dsl required services
            services.AddScoped<IPropertyBagBuilder, UserPropertyBagBuilder>();
            services.AddScoped<IPropertyBagBuilder, EndpointPropertyBagBuilder>();
            services.AddScoped<IPropertyBagBuilder, AuthorizationFilterContextPropertyBagBuilder>();
            services.AddScoped<DslAuthorizationContextFactory>();

            return new BaleaBuilder(services);
        }
    }
}
