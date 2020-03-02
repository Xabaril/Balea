![Balea CI](https://github.com/Xabaril/Balea/workflows/Balea%20CI/badge.svg)

# Authentication != Authorization

Authentication and authorization might be sound similar but both are distinct security processes in the world of identity and access management and understand the difference between these two concepts are the key to successfully implementing a good IAM solution.

While authentication is the act of verifing oneself, authorization is the process of verifing what you have access to, so coupling identity and access management in a single solution is not consider a good approach. Authentication is really good for provide a common identity across all applications while authorization is something that depends on each application, for these reason we should treat them indepentdly.

It's very common to see how people missues OIDC servers adding permissions into tokens and there are many reasons why this approach is a wrong solution:

- Permissions are something that depends on each application and sometimes depends on complex bussines rules.
- Permissions could change during the user session, so if you are using JWT tokens, you must be wait until the lifetime of the token expires in order to retrieve a new token with the permissions up to date.
- You should keep your tokens smalls because we have some well known restrictions such us URL Path Length Restrictions, bandwidth...

# What is Balea?

Balea is an authorization framework for ASP.NET Core developers that aims to help us to decoupling authentication and authorization in our web applications.

# Getting started

The authorization is defined as a JSON document or in a database.

We recommend using ASP.NET Core configuration store only for demos or simple applications. To install Balea open a console window and type the following command using the .NET Core CLI:

```txt
dotnet package add Balea.Configuration.Store
```

or using Powershell or Package Manager:

```txt
Install-Package Balea.Configuration.Store
```

or install via NuGet.

In order to store this information in a centralized store, we recommend using EntityFrameworkCore store:

```txt
dotnet package add Balea.EntityFrameworkCore.Store
```

or 

```txt
Install-Package Balea.EntityFrameworkCore.Store
```

In the **ConfigureServices** method of Startup.cs, register the Balea services:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services
        .AddBalea()
        .AddConfigurationStore(Configuration);
}
```

By default Balea use a configuration section called **Balea** but you can changed if you want:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services
        .AddBalea()
        .AddConfigurationStore(Configuration, key: "key name");
}
```

## Defining applications

Applications allows you to manage authorization in multiple different software projects. Each application has it's own unique roles and delegations. If you have a simple scenario where you only have one application, Balea give you a default application name called "default":

```json
{
  "Balea": {
    "applications": [
      {
        "name": "default"
      }
    ]
  }
}
```

Or you can create as many applications as you want:

```json
{
  "Balea": {
    "applications": [
      {
        "name": "hr"
      },
      {
        "name": "erp"
      }
    ]
  }
}
```

## Defining roles

Define roles is a straightforward proccess. Name the role, add a description, enable it and add permissions:

```json
{
  "Balea": {
    "applications": [
      {
        "name": "default",
        "roles": [
          {
            "name": "teacher",
            "description": "Teacher role",
            "enabled": true,
            "permissions": [
              "grades.edit",
              "grades.view"
            ]
          }
      }
    ]
  }
}
```

