using Volvoreta.Endpoints;

namespace Microsoft.AspNetCore.Builder
{
    public static class VolvoretaApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseVolvoreta(this IApplicationBuilder app)
        {
            return app.UseMiddleware<VolvoretaMiddleware>();
        }
    }
}
