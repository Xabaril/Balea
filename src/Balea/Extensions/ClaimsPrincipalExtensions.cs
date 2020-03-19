using Balea;
using System.Collections.Generic;
using System.Linq;

namespace System.Security.Claims
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetSubjectId(this ClaimsPrincipal principal, BaleaOptions options)
        {
            var claim = 
                principal.FindFirst(options.DefaultClaimTypeMap.SubjectClaimType) ??
                principal.FindFirst(options.DefaultClaimTypeMap.FallbackSubjectClaimType) ??
                throw new InvalidOperationException($"'{options.DefaultClaimTypeMap.SubjectClaimType}' or '{options.DefaultClaimTypeMap.FallbackSubjectClaimType}' claim is missing.");
            
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
