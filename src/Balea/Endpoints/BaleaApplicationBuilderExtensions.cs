using Balea.Endpoints;

namespace Microsoft.AspNetCore.Builder
{
    public static class BaleaApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseBalea(this IApplicationBuilder app)
        {
            return app.UseMiddleware<BaleaMiddleware>();
        }
    }
}
