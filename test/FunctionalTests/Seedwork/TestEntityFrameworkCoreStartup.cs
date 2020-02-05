using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Acheve.AspNetCore.TestHost.Security;
using Acheve.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace FunctionalTests.Seedwork
{
    public class TestEntityFrameworkCoreStartup
    {
        private readonly IConfiguration configuration;

        public TestEntityFrameworkCoreStartup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddVolvoreta()
                .AddEntityFrameworkCoreStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                    {
                        builder.UseSqlServer(configuration.GetConnectionString(ConnectionStrings.Default), sqlServerOptions =>
                        {
                            sqlServerOptions.MigrationsAssembly(typeof(TestEntityFrameworkCoreStartup).Assembly.FullName);
                        })
                        .UseLoggerFactory(LoggerFactory.Create(builder =>
                        {
                            builder.SetMinimumLevel(LogLevel.Information).AddConsole();
                        }));
                    };

                })
                .Services
                .AddAuthentication(setup =>
                {
                    setup.DefaultAuthenticateScheme = TestServerDefaults.AuthenticationScheme;
                    setup.DefaultChallengeScheme = TestServerDefaults.AuthenticationScheme;
                })
                .AddTestServer()
                .Services
                .AddMvc();
        }

        public void Configure(IApplicationBuilder app)
        {
            app
                .UseAuthentication()
                .UseVolvoreta()
                .UseRouting()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapDefaultControllerRoute();
                });
        }
    }
}
