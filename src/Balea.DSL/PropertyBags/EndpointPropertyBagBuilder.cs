using Balea.DSL.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Balea.DSL.PropertyBags
{
    class EndpointPropertyBagBuilder
        : IPropertyBagBuilder
    {
        private ILogger<EndpointPropertyBagBuilder> _logger;

        public EndpointPropertyBagBuilder(ILogger<EndpointPropertyBagBuilder> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public string BagName => "Resource";

        public Dictionary<string, object> Build(AuthorizationHandlerContext authorizationHandlerContext)
        {
            var entries = new Dictionary<string, object>();

            if (authorizationHandlerContext != null && authorizationHandlerContext.Resource is Endpoint endpoint)
            {
                _logger.PopulateEndpointPropertyBag(BagName);

                var actionDescriptor = endpoint.Metadata
                    .Where(m => typeof(ControllerActionDescriptor).IsAssignableFrom(m.GetType()))
                    .FirstOrDefault();

                if (actionDescriptor is ControllerActionDescriptor action)
                {
                    entries.Add("DisplayName", action.DisplayName);
                    entries.Add("Template", action.AttributeRouteInfo.Template);

                    foreach (var routeValue in action.RouteValues)
                    {
                        entries.Add(
                            key: CultureInfo.InvariantCulture.TextInfo.ToTitleCase(routeValue.Key),
                            value: routeValue.Value);
                    }
                }
            }

            return entries;
        }
    }
}
