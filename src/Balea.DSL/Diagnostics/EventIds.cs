using Microsoft.Extensions.Logging;

namespace Balea.DSL.Diagnostics
{
    internal static class EventIds
    {
        internal static readonly EventId PropertyBag = new EventId(9000, nameof(PropertyBag));
        internal static readonly EventId PropertyBagCantBePopulated = new EventId(9001, nameof(PropertyBagCantBePopulated));
    }
}
