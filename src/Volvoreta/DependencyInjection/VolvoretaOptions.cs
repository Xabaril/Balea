using System.Security.Claims;

namespace Volvoreta
{
    public class VolvoretaOptions
    {
        public string DefaultRoleClaimType { get; internal set; } = ClaimTypes.Role;
        public string DefaultApplicationName { get; internal set; } = VolvoretaConstants.DefaultApplicationName;

        public VolvoretaOptions SetDefaultRoleClaimType(string value)
        {
            DefaultRoleClaimType = value;
            return this;
        }

        public VolvoretaOptions SetDefaultApplicationName(string value)
        {
            DefaultApplicationName = value;
            return this;
        }
    }
}
