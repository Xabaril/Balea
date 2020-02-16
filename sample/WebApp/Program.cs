using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Balea.EntityFrameworkCore.Store.DbContexts;
using WebApp.Infrastucture.Data.Seeders;

namespace WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .MigrateDbContext<StoreDbContext>(db => BaleaSeeder.Seed(db))
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
            .ConfigureAppConfiguration(builder =>
            {
                builder.AddJsonFile("Balea.json", optional: false, reloadOnChange: true);
            });
    }
}
