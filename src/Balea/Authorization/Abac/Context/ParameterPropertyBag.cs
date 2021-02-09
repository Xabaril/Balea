﻿using Balea.Diagnostics;
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
        : IPropertyBag
    {
        private readonly Dictionary<string, (Type parameterType, StringValues parameterValues)> _entries = new Dictionary<string, (Type parameterType, StringValues parameterValues)>();
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<ParameterPropertyBag> _logger;

        ///<inheritdoc/>
        public string Name => "Parameters";

        ///<inheritdoc/>
        public object this[string propertyName]
        {
            get
            {
                //convert to the specified type discovered on the request information

                var parameterType = _entries[propertyName].parameterType;
                var parameterValue = _entries[propertyName].parameterValues
                    .FirstOrDefault();

                return TypeDescriptor.GetConverter(parameterType)
                    .ConvertFromString(parameterValue);
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
            return _entries
                .Where(c => c.Key == propertyName && c.Value.parameterValues.Contains(value.ToString()))
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
                    var values = _httpContextAccessor.HttpContext
                        .Request
                        .Query[parameter.Name];

                    if (values.Any())
                    {
                        _entries.Add(
                            key: CultureInfo.InvariantCulture.TextInfo.ToTitleCase(parameter.Name),
                            value: (parameter.ParameterType, values));
                    }
                }
            }
        }
    }
}
