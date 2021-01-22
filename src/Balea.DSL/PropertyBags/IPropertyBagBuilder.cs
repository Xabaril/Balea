using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace Balea.DSL.PropertyBags
{
    public interface IPropertyBagBuilder
    {
        string BagName { get; }

        Dictionary<string, object> Build(AuthorizationHandlerContext state);
    }
}
