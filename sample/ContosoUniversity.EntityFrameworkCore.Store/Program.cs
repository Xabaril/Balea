using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Balea.EntityFrameworkCore.Store.DbContexts;
using ContosoUniversity.EntityFrameworkCore.Store.Infrastructure.Data.Seeders;

namespace ContosoUniversity.EntityFrameworkCore.Store
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .MigrateDbContext<BaleaDbContext>(db => BaleaSeeder.Seed(db).Wait())
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
