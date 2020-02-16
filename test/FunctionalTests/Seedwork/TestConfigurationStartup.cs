using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Acheve.AspNetCore.TestHost.Security;
using Acheve.TestHost;

namespace FunctionalTests.Seedwork
{
    public class TestConfigurationStartup
    {
        private readonly IConfiguration configuration;

        public TestConfigurationStartup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddBalea(options =>
                {
                    options
                        .SetSourceRoleClaimType("sourceRole")
                        .SetBaleaRoleClaimType("BaleaRole");
                })
                .AddConfigurationStore(configuration)
                .Services
                .AddAuthentication(setup =>
                {
                    setup.DefaultAuthenticateScheme = TestServerDefaults.AuthenticationScheme;
                    setup.DefaultChallengeScheme = TestServerDefaults.AuthenticationScheme;
                })
                .AddTestServer(options =>
                {
                    options.RoleClaimType = "sourceRole";
                })
                .Services
                .AddMvc();
        }

        public void Configure(IApplicationBuilder app)
        {
            app
                .UseAuthentication()
                .UseBalea()
                .UseRouting()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapDefaultControllerRoute();
                });
        }
    }
}
