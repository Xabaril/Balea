using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Balea
{
    public class BaleaOptions
    {
        private readonly List<string> _schemes = new List<string>();

        public RequestDelegate UnauthorizedFallback { get; set; }
        public DefaultClaimTypeMap DefaultClaimTypeMap { get; set; } = new DefaultClaimTypeMap();
        public string ApplicationName { get; internal set; } = BaleaConstants.DefaultApplicationName;
        public IEnumerable<string> Schemes => _schemes.AsReadOnly();

        public BaleaOptions SetApplicationName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("The value can not be null or empty", nameof(value));
            }

            ApplicationName = value;
            return this;
        }

        public BaleaOptions AddAuthenticationSchemes(params string[] schemes)
        {
            _schemes.AddRange(schemes);

            return this;
        }
    }
}
