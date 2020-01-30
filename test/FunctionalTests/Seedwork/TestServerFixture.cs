using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Volvoreta.EntityFrameworkCore.Store.DbContexts;
using System.Collections.Generic;
using System.Linq;

namespace FunctionalTests.Seedwork
{
    public class TestServerFixture
    {
        public List<TestServer> Servers { get; private set; } = new List<TestServer>();

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
                        configure
                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                            .AddJsonFile("volvoreta.json", optional: false, reloadOnChange: true);
                    })
                    .Build();

                host.StartAsync().Wait();
                host.MigrateDbContext<StoreDbContext>((_, __) => { });
                Servers.Add(host.GetTestServer());
            }
        }
    }
}
