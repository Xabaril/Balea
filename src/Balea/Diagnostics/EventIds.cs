using Microsoft.Extensions.Logging;

namespace Balea.Diagnostics
{
    internal static class EventIds
    {
        public static readonly EventId AuthorizationPolicyFound = new EventId(220, nameof(AuthorizationPolicyFound));
        public static readonly EventId AuthorizationPolicyNotFound = new EventId(221, nameof(AuthorizationPolicyNotFound));
        public static readonly EventId CreatingAuthorizationPolicy = new EventId(222, nameof(CreatingAuthorizationPolicy));
        public static readonly EventId NoBaleaRolesForUser = new EventId(230, nameof(NoBaleaRolesForUser));
        public static readonly EventId ExecutingBaleaUnauthorizedFallback = new EventId(231, nameof(ExecutingBaleaUnauthorizedFallback));
        public static readonly EventId NoBaleaRolesForUserAndNoUnauthorizedFallback = new EventId(232, nameof(NoBaleaRolesForUserAndNoUnauthorizedFallback));
        public static readonly EventId PolicySucceed = new EventId(240, nameof(PolicySucceed));
        public static readonly EventId PolicyFail = new EventId(241, nameof(PolicyFail));
        internal static readonly EventId PropertyBag = new EventId(242, nameof(PropertyBag));
        internal static readonly EventId PropertyBagCantBePopulated = new EventId(243, nameof(PropertyBagCantBePopulated));
    }
}
