using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Xunit;

namespace FunctionalTests.Scenarios
{
    public class use_balea
    {
        [Fact]
        public void must_appear_before_app_use_authorization()
        {
            Action action = () => 
                Host.CreateDefaultBuilder()
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseStartup<Startup>();
                    })
                    .StartAsync()
                    .Wait();

            action.Should().Throw<InvalidOperationException>();
        }

        class Startup
        {
            public void ConfigureServices(IServiceCollection services)
            {
                
            }

            public void Configure(IApplicationBuilder app)
            {
                app.UseAuthorization().UseBalea();
            }
        }
    }
}
