using Balea.DSL.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Balea.DSL
{
    /// <summary>
    /// A property bag that collect information about user claims and allow to use this on 
    /// Abac authorizacion policy with the Subject accessor. This property bag map standard
    /// claim names to more simple name identifiers using the <see cref="ClaimMapping"/> dictionary.
    /// </summary>
    public class UserPropertyBag
        : IPropertyBag
    {
        /// <summary>
        /// The Claim mapping dictionary used to translate some standard claims like ClaimTypes.Role, ClaimTypesName to
        /// more simple name identifiers ( Subject.Role and Subject.Name). You can add or modify existing entries to 
        /// include new simplified name identifiers or modify existing mapping.
        /// </summary>
        public static Dictionary<string, IEnumerable<string>> ClaimMapping = new Dictionary<string, IEnumerable<string>>()
        {
            { "Role", new string[] { ClaimTypes.Role, "role" } },
            { "Name", new string[] { ClaimTypes.Name, "name" } },
            { "Sub", new string[] { ClaimTypes.NameIdentifier, "sub" } },
            { "Email", new string[] { ClaimTypes.Email, "email" } },
            { "GivenName", new string[] { ClaimTypes.GivenName, "given_name" } },
            { "FamilyName", new string[] { ClaimTypes.Surname, "family_name" } }
        };

        private IEnumerable<Claim> _claims;
        private readonly ILogger<UserPropertyBag> _logger;

        ///<inheritdoc/>
        public string Name { get; } = "Subject";

       ///<inheritdoc/>
        public object this[string propertyName]
        {
            get
            {
                IEnumerable<string> claimTypes = ClaimMapping.ContainsKey(propertyName) 
                    ? ClaimMapping[propertyName] : new string[] { propertyName };

                var value = _claims
                    .Where(c => claimTypes.Contains(c.Type))
                    .FirstOrDefault();

                if (value is object)
                {
                    return value.Value;
                }

                return string.Empty; // TODO: Thinking on semantic of NULL on DSL
            }
        }

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="logger">The logger to be used.</param>
        public UserPropertyBag(ILogger<UserPropertyBag> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _claims = new List<Claim>();
        }


        ///<inheritdoc/>
        public bool Contains(string propertyName, object value)
        {
            IEnumerable<string> claimTypes = ClaimMapping.ContainsKey(propertyName)
                   ? ClaimMapping[propertyName] : new string[] { propertyName };

            return _claims
                .Where(c => claimTypes.Contains(c.Type) && c.Value.Equals(value.ToString(), StringComparison.InvariantCultureIgnoreCase))
                .Any();
        }

        ///<inheritdoc/>
        public Task Initialize(AuthorizationHandlerContext authorizationHandlerContext)
        {
            if (authorizationHandlerContext != null && authorizationHandlerContext.User is ClaimsPrincipal claimsPrincipal)
            {
                _logger.PopulatePropertyBag(Name);
                _claims = claimsPrincipal.Claims;
            }
            else
            {
                _logger.PropertyBagCantBePopulated(Name);
            }

            return Task.CompletedTask;
        }
    }
}
