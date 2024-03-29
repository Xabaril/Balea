﻿using Balea;
using Balea.Abstractions;
using Balea.Authorization;
using Balea.Authorization.Abac;
using Balea.Authorization.Abac.Context;
using Balea.Authorization.Rbac;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.Extensions.Options;
using System;

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
            services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();

            services.AddScoped<IPermissionEvaluator, DefaultPermissionEvaluator>();
            services.AddScoped<IPropertyBag, UserPropertyBag>();
            services.AddScoped<IPropertyBag, ResourcePropertyBag>();
            services.AddScoped<IPropertyBag, ParameterPropertyBag>();
            services.AddScoped<AbacAuthorizationContextFactory>();

            services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();
            services.AddTransient<IAuthorizationHandler, AbacAuthorizationHandler>();
            services.AddTransient<IPolicyEvaluator, BaleaPolicyEvaluator>();

            return new BaleaBuilder(services);
        }
    }
}
