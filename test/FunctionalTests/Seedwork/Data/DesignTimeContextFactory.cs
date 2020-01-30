using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using Volvoreta.EntityFrameworkCore.Store.DbContexts;

namespace FunctionalTests.Seedwork.Data
{
    public class DesignTimeContextFactory : IDesignTimeDbContextFactory<StoreDbContext>
    {
        public StoreDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var builder = new DbContextOptionsBuilder<StoreDbContext>()
                .UseSqlServer(configuration.GetConnectionString("Default"), sqlServerOptions =>
                {
                    sqlServerOptions.MigrationsAssembly(typeof(DesignTimeContextFactory).Assembly.FullName);
                });

            return new StoreDbContext(builder.Options);
        }
    }
}
