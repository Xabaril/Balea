using Balea.DSL.AspNetCore;
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
    /// A property bag that collect information about the request path executed like the action name or controller name.
    /// </summary>
    public class ParameterPropertyBag
        : IPropertyBag
    {
        private readonly Dictionary<string, object> _entries = new Dictionary<string, object>();
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<ParameterPropertyBag> _logger;

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="httpContextAccessor">The http context accessor used to request parameters.</param>
        /// <param name="logger">The logger to be used.</param>
        public ParameterPropertyBag(IHttpContextAccessor httpContextAccessor, ILogger<ParameterPropertyBag> logger)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        ///<inheritdoc/>
        public object this[string propertyName]
        {
            get
            {
                //TODO: parameters will be multiple values and support for IN 
                return _entries[propertyName];
            }
        }

        ///<inheritdoc/>
        public string Name => "Parameters";

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
                    FillParameters(action);
                }
            }
            else if (authorizationHandlerContext != null && authorizationHandlerContext.Resource is AuthorizationFilterContext authorizationFilterContext)
            {
                if(authorizationFilterContext.ActionDescriptor is ControllerActionDescriptor action)
                {
                    FillParameters(action);
                }
            }
            else
            {
                _logger.PropertyBagCantBePopulated(Name);
            }

            return Task.CompletedTask;
        }

        void FillParameters(ControllerActionDescriptor action)
        {
            foreach (var parameter in action.Parameters.OfType<ControllerParameterDescriptor>())
            {
                if (parameter.ParameterInfo
                    .CustomAttributes
                    .Where(ca => typeof(AbacParameterAttribute).IsAssignableFrom(ca.AttributeType))
                    .Any())
                {
                    var value = _httpContextAccessor.HttpContext
                        .Request
                        .Query[parameter.Name];

                    _entries.Add(
                        key: CultureInfo.InvariantCulture.TextInfo.ToTitleCase(parameter.Name),
                        value: value.First());
                }
            }
        }
    }
}
