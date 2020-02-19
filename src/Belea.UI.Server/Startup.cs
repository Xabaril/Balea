using Balea.EntityFrameworkCore.Store.DbContexts;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Net.Mime;

namespace Belea.UI.Server
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddDbContext<StoreDbContext>(options =>
                {
                    options.UseSqlServer(configuration.GetConnectionString("Balea"), sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(typeof(Startup).Assembly.FullName);
                    });
                })
                .AddMediatR(typeof(Balea.UI.Api.Configuration))
                .AddResponseCompression(options =>
                {
                    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                        new[] { MediaTypeNames.Application.Octet }
                    );
                })
                .AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseResponseCompression();

            if (env.IsDevelopment())
            {
                app
                    .UseDeveloperExceptionPage()
                    .UseBlazorDebugging();
            }

            app
                .UseStaticFiles()
                .UseRouting()
                .UseClientSideBlazorFiles<Balea.UI.Client.Program>()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.MapFallbackToClientSideBlazor<Balea.UI.Client.Program>("index.html");
                });
        }
    }
}
