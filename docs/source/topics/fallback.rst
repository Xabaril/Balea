Configure a fallback mechanisim for unauthorized users
======================================================

Even authenticated, not all users are authorized to access to all applications. Out-of-the-box Balea provides an authorization fallback mechanism to decide what to do with the unauhtorized users.

> In `samples/WebApp <https://github.com/Xabaril/Balea/tree/master/sample/WebApp>`_ you'll find an example of how to configure this fallback mechanism.

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

We can modify the code like this::

      services
        .AddBalea(options =>
        {
          options.UnauthorizedFallback = AuthorizationFallbackAction.RedirectToAction("Home","Denied");
        })
        .AddConfigurationStore(Configuration);