using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Microsoft.Extensions.Hosting
{
    public static class IHostExtensions
    {
        public static IHost MigrateDbContext<TContext>(this IHost host, Action<TContext> seed = null) where TContext : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                try
                {
                    var context = scope.ServiceProvider.GetService<TContext>();

                    if (context != null)
                    {
                        context.Database.Migrate();
                        seed?.Invoke(context);
                    }
                }
                catch (Exception exception)
                {
                    var logger = scope.ServiceProvider.GetService<ILogger<TContext>>();

                    logger.LogError(exception, $"An error ocurred while migrating database for ${nameof(TContext)}");
                }

                return host;
            }
        }
    }
}
