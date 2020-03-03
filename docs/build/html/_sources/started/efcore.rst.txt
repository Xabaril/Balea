Using EntityFramework Core store
================================

Balea was designed to be extensible and one of the extensibility points is the storage. This article shows you how to configure Balea in order to use EntityFrameworkCore as the storage mechanism rather than using ASP.NET Configuration store.

> In `samples/WebApp <https://github.com/Xabaril/Balea/tree/master/sample/WebAppEfCore>`_ you'll find a complete Balea example in ASP.NET Core.

Configuring the store
---------------------

To install Balea open a console window and type the following command using the .NET Core CLI::

        dotnet package add Balea.EntityFrameworkCore.Store

or using Powershell or Package Manager::

        Install-Package Balea.EntityFrameworkCore.Store

or install via NuGet.

In the **ConfigureServices** method of Startup.cs, register the Balea services::

        services                     
            .AddBalea()
            .AddEntityFrameworkCoreStore(options =>
            {
                options.ConfigureDbContext = builder =>
                {
                    builder.UseSqlServer(Configuration.GetConnectionString("Default"), sqlServerOptions =>
                    {
                        sqlServerOptions.MigrationsAssembly(typeof(Startup).Assembly.FullName);
                    });
                };
            })

``AddBalea`` method allows you to register the set of services that Balea needs to works. The ``AddEntityFrameworkCoreStore`` method registers the EntityFrameworkCore store and also give the opportunity to configure your favorite `provider <https://docs.microsoft.com/en-us/ef/core/providers/?tabs=dotnet-core-cli>`_

Adding the initial migration
----------------------------

Balea.EntityFrameworkCore.Store contains all the entities needed to store all the Balea configuration in a database. This entities could be changed over the time, so you are responsible to upgrade your own database. To manage this changes one approach is using EntityFramework Core migrations. To create the initial migration in your web application open a console window and type the following commands using the .NET Core CLI::

        dotnet ef migrations add Initial -c StoreDbContext -o "Migrations\Balea" -s "Path to your proyect"
        dotnet ef database update -s "Path to your proyect"

or using Powershell or Package Manager::

        Add-Migration Initial -OutputDir "Migrations\Balea· -StartupProject "Path to your project"
        Update-Database -StartupProject "Path to your project"

The database should be created and you should be able to connect using SQL Server Management, Visual Studio or another tool:

.. image:: ../images/databaseschema.png

