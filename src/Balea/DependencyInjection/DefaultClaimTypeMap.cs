using System.Security.Claims;

namespace Balea
{
    public class DefaultClaimTypeMap
    {
        public string SourceRoleClaimType { get; set; } = ClaimTypes.Role;
        public string BaleaNameClaimType { get; set; } = ClaimTypes.Name;
        public string BaleaRoleClaimType { get; set; } = ClaimTypes.Role;
        public string BaleaPermissionClaimType { get; set; } = BaleaClaims.Permission;
    }
}
