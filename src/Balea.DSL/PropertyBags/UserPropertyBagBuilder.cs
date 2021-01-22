using Balea.DSL.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Balea.DSL.PropertyBags
{
    class UserPropertyBagBuilder
        : IPropertyBagBuilder
    {
        private readonly ILogger<UserPropertyBagBuilder> _logger;

        public UserPropertyBagBuilder(ILogger<UserPropertyBagBuilder> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public string BagName => "Subject";

        public Dictionary<string, object> Build(AuthorizationHandlerContext authorizationHandlerContext)
        {
            var entries = new Dictionary<string, object>();

            if (authorizationHandlerContext != null && authorizationHandlerContext.User is ClaimsPrincipal claimsPrincipal)
            {
                _logger.PopulateUserPropertyBag(BagName);

                return claimsPrincipal.Claims
                    .ToDictionary(c => c.Type, c => (object)c.Value);
            }

            return entries;
        }
    }
}
