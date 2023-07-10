using Balea.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Balea.Authorization.Abac.Context
{
    /// <summary>
    /// A property bag that collect information about the request path executed like the action name or controller name.
    /// </summary>
    public class ParameterPropertyBag
        : IAspNetPropertyBag
	{
        private readonly Dictionary<string, (Type parameterType, StringValues parameterValues)> _entries
            = new Dictionary<string, (Type parameterType, StringValues parameterValues)>(StringComparer.InvariantCultureIgnoreCase);

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<ParameterPropertyBag> _logger;

        ///<inheritdoc/>
        public string Name => "Parameters";

        ///<inheritdoc/>
        public object this[string propertyName]
        {
            get
            {
                // You use equal expression for this property bag but the item can 
                // contain multiple elements, probably in this case CONTAINS will be 
                // a more apropiate operator but we need to solve this scenario, for that
                // we use the first element on the collection if exist converted to the specified type
                // discovered on the ControllerParameterDescription.

                if (_entries.ContainsKey(propertyName))
                {
                    var parameterType = _entries[propertyName].parameterType;
                    var parameterValue = _entries[propertyName].parameterValues
                        .FirstOrDefault();

                    return TypeDescriptor.GetConverter(parameterType)
                        .ConvertFromString(parameterValue);
                }

                throw new ArgumentException($"The property name {propertyName} does not exist on the {Name}  property bag.");
            }
        }

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
        public bool Contains(string propertyName, object value)
        {
            // This method is used when the grammar use CONTAINS operator, in this we don't need
            // to perfrom conversion type operations and check only if the StringValues of the selected parameter 
            // exist and the value is on this collection

            return _entries
                .Where(c => c.Key.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase) && c.Value.parameterValues.Contains(value.ToString()))
                .Any();
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
                    FillParameters(action);
                }
            }
            else if (authorizationHandlerContext != null && authorizationHandlerContext.Resource is AuthorizationFilterContext authorizationFilterContext)
            {
                if (authorizationFilterContext.ActionDescriptor is ControllerActionDescriptor action)
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
            // just collect all the parameter values on the request on and type info decorated
            // with attribute and use it on this property bag

            foreach (var parameter in action.Parameters.OfType<ControllerParameterDescriptor>())
            {
                if (parameter.ParameterInfo
                    .CustomAttributes
                    .Where(attribute => typeof(AbacParameterAttribute).IsAssignableFrom(attribute.AttributeType))
                    .Any())
                {
                    Log.AbacDiscoverPropertyBagParameter(_logger, parameter.Name, parameter.ParameterType.Name);

                    var parameterAttribute = (AbacParameterAttribute)parameter.ParameterInfo
                        .GetCustomAttributes(typeof(AbacParameterAttribute), false)
                        .SingleOrDefault();

                    StringValues values = default;

                    if (_httpContextAccessor.HttpContext.Request
                        .Query
                        .ContainsKey(parameter.Name))
                    {
                        // find value on query string
                        values = _httpContextAccessor.HttpContext
                            .Request
                            .Query[parameter.Name];
                    }

                    if (!HttpMethods.IsGet(_httpContextAccessor.HttpContext.Request.Method)
                        &&
                        !HttpMethods.IsHead(_httpContextAccessor.HttpContext.Request.Method)
                        &&
                        !HttpMethods.IsOptions(_httpContextAccessor.HttpContext.Request.Method))
                    {
                        //find the value on body form if is not a get, head or options http method
                        values = _httpContextAccessor.HttpContext
                            .Request
                            .Form[parameter.Name];
                    }

                    _entries.Add(
                        key: parameterAttribute.Name ?? CultureInfo.InvariantCulture.TextInfo.ToTitleCase(parameter.Name),
                        value: (parameter.ParameterType, values));
                }
            }
        }
    }
}
