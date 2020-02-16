using System;
using System.Security.Claims;

namespace Volvoreta
{
    public class VolvoretaOptions
    {
        public string SourceRoleClaimType { get; internal set; } = ClaimTypes.Role;
        public string VolvoretaNameClaimType { get; internal set; } = ClaimTypes.Name;
        public string VolvoretaRoleClaimType { get; internal set; } = ClaimTypes.Role;
        public string ApplicationName { get; internal set; } = VolvoretaConstants.DefaultApplicationName;

        public VolvoretaOptions SetSourceRoleClaimType(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("The value can not be null or empty", nameof(value));
            }

            SourceRoleClaimType = value;
            return this;
        }

        public VolvoretaOptions SetVolvoretaNameClaimType(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("The value can not be null or empty", nameof(value));
            }

            VolvoretaNameClaimType = value;
            return this;
        }

        public VolvoretaOptions SetVolvoretaRoleClaimType(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("The value can not be null or empty", nameof(value));
            }

            VolvoretaRoleClaimType = value;
            return this;
        }

        public VolvoretaOptions SetApplicationName(string value)
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
