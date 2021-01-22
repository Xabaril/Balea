using Microsoft.Extensions.Logging;

namespace Balea.DSL.Diagnostics
{
    internal static class EventIds
    {
        internal static readonly EventId UserPropertyBag = new EventId(9000, nameof(UserPropertyBag));
        internal static readonly EventId EndpointPropertyBag = new EventId(9001, nameof(UserPropertyBag));
        internal static readonly EventId AuthorizationFilterContextPropertyBag = new EventId(9002, nameof(UserPropertyBag));
    }
}
