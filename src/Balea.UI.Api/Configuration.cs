using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Balea.UI.Api
{
    public static class Configuration
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services
                .AddAuthorization()
                .AddCustomProblemDetails();
        }

        public static void Configure(
            IApplicationBuilder app,
            Func<IApplicationBuilder,IApplicationBuilder> preConfigure,
            Func<IApplicationBuilder, IApplicationBuilder> postConfigure)
        {
            var builder = preConfigure(app)
                .UseProblemDetails();

            postConfigure(builder);
        }
    }
}
