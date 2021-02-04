using Balea.DSL.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Balea.DSL
{
    /// <summary>
    /// A property bag that collect information about the resource executed like the action name or controller name.
    /// If the user is using ASP.NET Core endpoints this information is collected from the <see cref="Endpoint"/> else is collected from
    /// <see cref="AuthorizationFilterContext"/> instance.
    /// </summary>
    public class ResourcePropertyBag
        : IPropertyBag
    {
        const string DisplayName = nameof(DisplayName);
        const string Template = nameof(Template);

        private readonly Dictionary<string, object> _entries = new Dictionary<string, object>();
        private readonly ILogger<ResourcePropertyBag> _logger;

        ///<inheritdoc/>
        public string Name => "Resource";

        ///<inheritdoc/>
        public object this[string propertyName]
        {
            get
            {
                if (_entries.ContainsKey(propertyName))
                {
                    return _entries[propertyName];
                }

                return string.Empty; // TODO: check the null semantic on DSL
            }
        }

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="logger">The logger to be used.</param>
        public ResourcePropertyBag(ILogger<ResourcePropertyBag> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        ///<inheritdoc/>
        public Task Initialize(AuthorizationHandlerContext authorizationHandlerContext)
        {
            _logger.PopulatePropertyBag(Name);

            if (authorizationHandlerContext != null && authorizationHandlerContext.Resource is Endpoint endpoint)
            {
                var actionDescriptor = endpoint.Metadata
                    .Where(m => typeof(ControllerActionDescriptor).IsAssignableFrom(m.GetType()))
                    .FirstOrDefault();

                if (actionDescriptor is ControllerActionDescriptor action)
                {  
                    _entries.Add(DisplayName, action.DisplayName);

                    if (action.AttributeRouteInfo is object)
                    {
                        _entries.Add(Template, action.AttributeRouteInfo.Template);
                    }

                    foreach (var routeValue in action.RouteValues)
                    {
                        _entries.Add(
                            key: CultureInfo.InvariantCulture.TextInfo.ToTitleCase(routeValue.Key),
                            value: routeValue.Value);
                    }
                }
            }
            else if (authorizationHandlerContext != null && authorizationHandlerContext.Resource is AuthorizationFilterContext authorizationFilterContext)
            {
                _entries.Add(DisplayName, authorizationFilterContext.ActionDescriptor.DisplayName);

                if (authorizationFilterContext.ActionDescriptor.AttributeRouteInfo is object)
                {
                    _entries.Add(Template, authorizationFilterContext.ActionDescriptor.AttributeRouteInfo.Template);
                }

                foreach (var routeValue in authorizationFilterContext.ActionDescriptor.RouteValues)
                {
                    _entries.Add(
                        key: CultureInfo.InvariantCulture.TextInfo.ToTitleCase(routeValue.Key),
                        value: routeValue.Value);
                }
            }
            else
            {
                _logger.PropertyBagCantBePopulated(Name);
            }

            return Task.CompletedTask;
        }
    }
}
