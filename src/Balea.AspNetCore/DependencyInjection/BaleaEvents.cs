using Microsoft.AspNetCore.Http;

namespace Balea
{
    public class BaleaEvents
    {
        public RequestDelegate UnauthorizedFallback { get; set; }
    }
}
