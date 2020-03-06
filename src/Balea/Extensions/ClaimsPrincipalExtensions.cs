using Balea;
using System.Collections.Generic;
using System.Linq;

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
                throw new InvalidOperationException("sub claim is missing.");
            }
            
            return claim.Value;
        }

        public static IEnumerable<string> GetClaimValues(
            this ClaimsPrincipal principal,
            string claimType)
        {
            return principal.FindAll(claimType)
                .Select(x => x.Value);
        }
    }
}
