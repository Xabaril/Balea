using System.Collections.Generic;
using System.Linq;

namespace System.Security.Claims
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetSubjectId(this ClaimsPrincipal principal, string type)
        {
            var claim = principal
                .FindFirst(type);

            if (claim == null)
            {
                throw new InvalidOperationException("sub claim is missing");
            }
            
            return claim.Value;
        }

        public static IEnumerable<string> GetRoleClaimValues(
            this ClaimsPrincipal principal,
            string roleClaimType)
        {
            return principal.FindAll(roleClaimType)
                .Select(x => x.Value);
        }
    }
}
