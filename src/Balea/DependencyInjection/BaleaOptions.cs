using System;
using System.Security.Claims;

namespace Balea
{
    public class BaleaOptions
    {
        public string SourceRoleClaimType { get; internal set; } = ClaimTypes.Role;
        public string SourceNameIdentifierClaimType { get; internal set; } = ClaimTypes.NameIdentifier;
        public string BaleaNameClaimType { get; internal set; } = ClaimTypes.Name;
        public string BaleaRoleClaimType { get; internal set; } = ClaimTypes.Role;
        public string BaleaPermissionClaimType { get; internal set; } = BaleaClaims.Permission;
        public string ApplicationName { get; internal set; } = BaleaConstants.DefaultApplicationName;

        public BaleaOptions SetSourceRoleClaimType(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("The value can not be null or empty", nameof(value));
            }

            SourceRoleClaimType = value;
            return this;
        }

        public BaleaOptions SetSourceNameIdentitfierClaimType(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("The value can not be null or empty", nameof(value));
            }

            SourceNameIdentifierClaimType = value;
            return this;
        }

        public BaleaOptions SetBaleaNameClaimType(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("The value can not be null or empty", nameof(value));
            }

            BaleaNameClaimType = value;
            return this;
        }

        public BaleaOptions SetBaleaRoleClaimType(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("The value can not be null or empty", nameof(value));
            }

            BaleaRoleClaimType = value;
            return this;
        }

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
