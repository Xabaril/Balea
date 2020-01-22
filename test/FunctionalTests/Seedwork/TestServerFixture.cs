using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;

namespace FunctionalTests.Seedwork
{
    public class TestServerFixture
    {
        private IHost _host;

        public TestServer Server { get; private set; }

        public TestServerFixture()
        {
            InitializeTestServer();
        }

        private void InitializeTestServer()
        {
            _host = new HostBuilder()
                .ConfigureWebHost(configure =>
                {
                    configure
                        .ConfigureServices(services =>
                            services.AddSingleton<IServer>(serviceProvider =>
                                new TestServer(serviceProvider)
                            )
                        )
                        .UseStartup<TestStartup>();
                })
                .ConfigureAppConfiguration(configure =>
                {
                    configure.AddJsonFile("volvoreta.json", optional: false, reloadOnChange: true);
                })
                .Build();

            _host.StartAsync().Wait();

            Server = _host.GetTestServer();
        }
    }
}
