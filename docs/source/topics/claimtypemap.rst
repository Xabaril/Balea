Configure claim types map
=========================

There is an old history about claim types and the reason why some times authorization does not work as we expected. We recommend you to read this awesome `blog post <https://leastprivilege.com/2016/08/21/why-does-my-authorize-attribute-not-work/>`_ about it before to continue reading.  

The problem is basically that Microsoft has their owns claim type names for RoleClaimType (http://schemas.microsoft.com/ws/2008/06/identity/claims/role) and NameClaimType (http://schemas.microsoft.com/ws/2008/06/identity/claims/name) and of course their own JWT validation library which does not follow the more modern standard OpenID Connect claim `types <https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims>`_. 

> In `samples/WebApp <https://github.com/Xabaril/Balea/tree/master/sample/WebAppEfCoreOidc>`_ you'll find an example of how to configure the mappings.

Configure the AuthorizationFallback
-----------------------------------

To configure the authorization fallback, in the method ``AddBalea`` you have a parameter for the fallback: 

In the **ConfigureServices** method of Startup.cs, register the Balea services::

      services
        .AddBalea(options =>
        {
          options.UnauthorizedFallback = (context) =>
          {
              context.Response.StatusCode = StatusCodes.Status403Forbidden;
              return Task.CompletedTask;
          };
        })
        .AddConfigurationStore(Configuration);

The ``UnauthorizedFallback`` is a `RequestDelegate <https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http.requestdelegate?view=aspnetcore-3.1>`_ so you can configure the behavior when user is not authorized.

Out-of-the-box Balea provides a ``AuthorizationFallbackAction`` class that defines common fallback actions to be used when user is not authorized:

    * Redirect result to MVC action::
        
        public static RequestDelegate RedirectToAction(string controllerName, string actionName)

    * Redirect result::
        
        public static RequestDelegate RedirectTo(string uri)

    * Forbidden status response::
        
        public static RequestDelegate Forbidden

We can modify the code like this::

      services
        .AddBalea(options =>
        {
          options.UnauthorizedFallback = AuthorizationFallbackAction.Forbidden;
        })
        .AddConfigurationStore(Configuration);