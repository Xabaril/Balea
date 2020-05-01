using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Balea.EntityFrameworkCore.Store.DbContexts;
using System.Collections.Generic;
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
        private readonly Dictionary<Type, IHost> _hosts = new Dictionary<Type, IHost>();
        private readonly Dictionary<Type, TestServerInfo> _servers = new Dictionary<Type, TestServerInfo>();
        public IReadOnlyCollection<TestServerInfo> Servers => _servers.Values;

        public TestServerFixture()
        {
            InitializeTestServer();
        }

        private void InitializeTestServer()
        {
            var startups = new Tuple<Type, bool>[]
            {
                new Tuple<Type, bool>(typeof(TestConfigurationStartup), false),// Not support schemes
                new Tuple<Type, bool>(typeof(TestEntityFrameworkCoreStartup), false), // Not support schemes
                new Tuple<Type, bool>(typeof(TestConfigurationWithSchemesStartup), true) // Support schemes
            };

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
                            .UseStartup(startup.Item1);
                    })
                    .ConfigureAppConfiguration(configure =>
                    {
                        CreateTestConfiguration(configure);
                    })
                    .Build();

                host.StartAsync().Wait();
                host.MigrateDbContext<BaleaDbContext>((_, __) => { });
                _hosts.Add(startup.Item1, host);
                _servers.Add(startup.Item1, new TestServerInfo(host.GetTestServer(), startup.Item2));
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

        public async Task ExecuteDbContextAsync(Func<BaleaDbContext, Task> func)
        {
            await ExecuteScopeAsync(sp => func(sp.GetRequiredService<BaleaDbContext>()));
        }

        public static void ResetDatabase()
        {
            _checkpoint.Reset(
                CreateTestConfiguration(new ConfigurationBuilder())
                    .Build()
                    .GetConnectionString(ConnectionStrings.Default)
            ).Wait();
        }

        private static IConfigurationBuilder CreateTestConfiguration(IConfigurationBuilder builder)
        {
            return builder
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("balea.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
        }
    }

    public class TestServerInfo
    {
        public TestServerInfo(TestServer testServer, bool supportSchemes)
        {
            TestServer = testServer;
            SupportSchemes = supportSchemes;
        }

        public TestServer TestServer { get; }

        public bool SupportSchemes { get; }
    }
}
