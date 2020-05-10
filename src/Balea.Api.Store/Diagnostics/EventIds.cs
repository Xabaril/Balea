using Microsoft.Extensions.Logging;

namespace Balea.Server.Diagnostics
{
    internal static class EventIds
    {
        internal static readonly EventId ClientAuthorizationRequest = new EventId(8000, nameof(ClientAuthorizationRequest));
        internal static readonly EventId ClientAuthorizationResponse = new EventId(8001, nameof(ClientAuthorizationResponse));
        internal static readonly EventId CacheHit = new EventId(8003, nameof(CacheHit));
        internal static readonly EventId CacheMiss = new EventId(8004, nameof(CacheMiss));
    }
}
