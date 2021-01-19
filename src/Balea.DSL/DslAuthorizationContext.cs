using System.Collections.Generic;

namespace Balea.DSL
{
    public class DslAuthorizationContext
    {
        public Dictionary<string, object> Subject { get; set; }

        public Dictionary<string, object> Resource { get; set; }
    }
}
