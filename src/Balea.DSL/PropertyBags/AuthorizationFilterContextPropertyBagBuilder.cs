using Balea.DSL.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Balea.DSL.PropertyBags
{
    class AuthorizationFilterContextPropertyBagBuilder
        : IPropertyBagBuilder
    {
        private readonly ILogger<AuthorizationFilterContextPropertyBagBuilder> _logger;

        public AuthorizationFilterContextPropertyBagBuilder(ILogger<AuthorizationFilterContextPropertyBagBuilder> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public string BagName => "Resource";

        public Dictionary<string, object> Build(AuthorizationHandlerContext authorizationHandlerContext)
        {
            var entries = new Dictionary<string, object>();

            if (authorizationHandlerContext != null && authorizationHandlerContext.Resource is AuthorizationFilterContext authorizationFilterContext)
            {
                _logger.PopulateEndpointPropertyBag(BagName);

                entries.Add("DisplayName", authorizationFilterContext.ActionDescriptor.DisplayName);
                entries.Add("Template", authorizationFilterContext.ActionDescriptor.AttributeRouteInfo.Template);

                foreach (var routeValue in authorizationFilterContext.ActionDescriptor.RouteValues)
                {
                    entries.Add(
                        key: CultureInfo.InvariantCulture.TextInfo.ToTitleCase(routeValue.Key),
                        value: routeValue.Value);
                }
            }

            return entries;
        }
    }
}
