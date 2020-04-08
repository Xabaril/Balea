using Microsoft.Extensions.Logging;

namespace Balea.Diagnostics
{
    internal static class EventIds
    {
        public static readonly EventId BaleaMiddlewareThrow = new EventId(220, nameof(BaleaMiddlewareThrow));
        public static readonly EventId BaleaMiddlewareAuthorizationSuccess = new EventId(221, nameof(BaleaMiddlewareAuthorizationSuccess));
        public static readonly EventId BaleaMiddlewareActiveDelegation = new EventId(222, nameof(BaleaMiddlewareActiveDelegation));
    }
}
