using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components.Builder;

namespace Balea.Client
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {

        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
