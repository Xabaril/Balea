using System.Security.Claims;

namespace Balea
{
    public class DefaultClaimTypeMap
    {
        public string RoleClaimType { get; set; } = ClaimTypes.Role;
        public string NameClaimType { get; set; } = ClaimTypes.Name;
        public string PermissionClaimType { get; set; } = BaleaClaims.Permission;
    }
}
