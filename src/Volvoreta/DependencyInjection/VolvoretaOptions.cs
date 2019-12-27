using Microsoft.AspNetCore.Authorization;
using System;
using System.Security.Claims;

namespace Microsoft.Extensions.DependencyInjection
{
    public class VolvoretaOptions
    {
        internal string DefaultRoleClaimType = ClaimTypes.Role;

        public VolvoretaOptions SetDefaultRoleClaimType(string value)
        {
            DefaultRoleClaimType = value;
            return this;
        }

        public Action<AuthorizationOptions> ConfigureAuthorization { get; set; }
    }
}
