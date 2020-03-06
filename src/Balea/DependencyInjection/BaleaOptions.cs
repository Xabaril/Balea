using Microsoft.AspNetCore.Http;
using System;

namespace Balea
{
    public class BaleaOptions
    {
        public RequestDelegate UnauthorizedFallback { get; set; }
        public DefaultClaimTypeMap DefaultClaimTypeMap { get; set; } = new DefaultClaimTypeMap();
        public string ApplicationName { get; internal set; } = BaleaConstants.DefaultApplicationName;

        public BaleaOptions SetApplicationName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("The value can not be null or empty", nameof(value));
            }

            ApplicationName = value;
            return this;
        }
    }
}
