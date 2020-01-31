using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Volvoreta.EntityFrameworkCore.Store.DbContexts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Respawn;

namespace FunctionalTests.Seedwork
{
    public class TestServerFixture
    {
        private static Checkpoint _checkpoint = new Checkpoint
        {
            TablesToIgnore = new[] { "__EFMigrationsHistory" },
            WithReseed = true
        };
        private Dictionary<Type, IHost> _hosts = new Dictionary<Type, IHost>();
        private Dictionary<Type, TestServer> _servers = new Dictionary<Type, TestServer>();
        public IReadOnlyCollection<TestServer> Servers => _servers.Values;

        public TestServerFixture()
        {
            InitializeTestServer();
        }

        private void InitializeTestServer()
        {
            var startups = typeof(TestServerFixture)
                .Assembly
                .GetTypes()
                .Where(t => t.Name.EndsWith("Startup"));

            foreach (var startup in startups)
            {
                var host = new HostBuilder()
                    .ConfigureWebHost(configure =>
                    {
                        configure
                            .ConfigureServices(services =>
                                services.AddSingleton<IServer>(serviceProvider =>
                                    new TestServer(serviceProvider)
                                )
                            )
                            .UseStartup(startup);
                    })
                    .ConfigureAppConfiguration(configure =>
                    {
                        CreateTestConfiguration(configure);
                    })
                    .Build();

                host.StartAsync().Wait();
                host.MigrateDbContext<StoreDbContext>((_, __) => { });
                _hosts.Add(startup, host);
                _servers.Add(startup, host.GetTestServer());
            }
        }

        public async Task ExecuteScopeAsync(Func<IServiceProvider, Task> func)
        {
            using (var scope = _hosts[typeof(TestEntityFrameworkCoreStartup)]
                .Services
                .GetService<IServiceScopeFactory>()
                .CreateScope())
            {
                await func(scope.ServiceProvider);
            }
        }

        public async Task ExecuteDbContextAsync(Func<StoreDbContext, Task> func)
        {
            await ExecuteScopeAsync(sp => func(sp.GetRequiredService<StoreDbContext>()));
        }

        public static void ResetDatabase()
        {
            _checkpoint.Reset(
                CreateTestConfiguration(new ConfigurationBuilder())
                    .Build()
                    .GetConnectionString(ConnectionStrings.Default)
            );
        }

        private static IConfigurationBuilder CreateTestConfiguration(IConfigurationBuilder builder)
        {
            return builder
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("volvoreta.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
        }
    }
}
