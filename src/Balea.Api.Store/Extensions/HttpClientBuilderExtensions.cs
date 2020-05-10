using System.Net.Http;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class HttpClientBuilderExtensions
    {
        public static IHttpClientBuilder AddIfHttpMessageHandler<THandler>(this IHttpClientBuilder builder, bool condition) where THandler : DelegatingHandler
        {
            return condition
                ? builder.AddHttpMessageHandler<THandler>()
                : builder;
        }
    }
}
