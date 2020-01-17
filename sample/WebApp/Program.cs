using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Volvoreta.EntityFrameworkCore.Store.DbContexts;
using WebApp.Infrastratucture.Data.Seeders;

namespace WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .MigrateDbContext<StoreDbContext>(db => VolvoretaSeeder.Seed(db))
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
                builder.AddJsonFile("volvoreta.json", optional: false, reloadOnChange: true);
            });
    }
}
