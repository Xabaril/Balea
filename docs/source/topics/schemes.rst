Use Balea only in specific schemes
======================================================

When you are using more than one scheme in your application you might need to run Balea only in certain schemes. For example, if you are hosting an Mvc application authenticated with cookies and some Api endpoints authenticated with tokens in the same host you may want to use Balea only in your Api with the JwtBearer scheme.

To configure Balea to run only in specific schemes you should add the schemes in the Balea configuration in the **ConfigureServices** method:

      services
        .AddBalea(options =>
        {
            options.AddAuthenticationSchemes("Bearer");
        })
        .AddConfigurationStore(Configuration);

If no schemes registered in Balea it will run for the default authentication scheme configured.

If there are some authenticated schemes configured in Balea, it will run only in those specific schemes.