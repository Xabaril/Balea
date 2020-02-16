using System.Collections.Generic;
using System.Linq;
using Balea;

namespace System.Security.Claims
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetSubjectId(this ClaimsPrincipal principal)
        {
            var claim = principal
                .FindFirst(BaleaClaims.Subject);

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
