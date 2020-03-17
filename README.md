![Balea CI](https://github.com/Xabaril/Balea/workflows/Balea%20CI/badge.svg) 

[![Nuget](https://img.shields.io/nuget/v/balea?label=balea)](https://www.nuget.org/packages/Balea/) [![Nuget](https://img.shields.io/nuget/v/balea.configuration.store?label=baleaconfigurationstore)](https://www.nuget.org/packages/Balea.Configuration.Store/) [![Nuget](https://img.shields.io/nuget/v/balea.entityframeworkcore.store?label=baleaefcorestore)](https://www.nuget.org/packages/Balea.EntityFrameworkCore.Store/) 

[![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/balea?color=yellow&label=balea%20preview)](https://www.nuget.org/packages/Balea/) [![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/balea.configuration.store?color=yellow&label=baleaconfigurationstore%20preview)](https://www.nuget.org/packages/Balea.Configuration.Store/) [![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/balea.entityframeworkcore.store?color=yellow&label=baleaefcorestore%20preview)](https://www.nuget.org/packages/Balea.EntityFrameworkCore.Store/) 

# Authentication != Authorization

Authentication and authorization might be sound similar but both are distinct security processes in the world of identity and access management and understand the difference between these two concepts are the key to successfully implementing a good IAM solution.

While authentication is the act of verifing oneself, authorization is the process of verifing what you have access to, so coupling identity and access management in a single solution is not consider a good approach. Authentication is really good for provide a common identity across all applications while authorization is something that depends on each application, for these reason we should treat them indepentdly.

It's very common to see how people missues OIDC servers adding permissions into tokens and there are many reasons why this approach is a wrong solution:

- Permissions are something that depends on each application and sometimes depends on complex bussines rules.
- Permissions could change during the user session, so if you are using JWT tokens, you must be wait until the lifetime of the token expires in order to retrieve a new token with the permissions up to date.
- You should keep your tokens smalls because we have some well known restrictions such us URL Path Length Restrictions, bandwidth...

# What is Balea?

Balea is an authorization framework for ASP.NET Core developers that aims to help us to decoupling authentication and authorization in our web applications.

For project documentation, please visit [readthedocs](https://balea.readthedocs.io).

## How to build

Balea is built against the latest NET Core 3.

- [Install](https://www.microsoft.com/net/download/core#/current) the [required](https://github.com/Xabaril/Balea/blob/master/global.json) .NET Core SDK
- Run [build.ps1](https://github.com/Xabaril/Balea/blob/master/build.ps1) in the root of the repo.

## Acknowledgements

Balea is built using the following great open source projects and free services:

- [ASP.NET Core](https://github.com/aspnet)
- [XUnit](https://xunit.github.io/)
- [Fluent Assertions](http://www.fluentassertions.com/)

..and last but not least a big thanks to all our [contributors](https://github.com/Xabaril/Balea/graphs/contributors)!

## Code of conduct

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).  For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.
