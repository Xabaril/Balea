﻿using System.Collections.Generic;
using System.Linq;
using Volvoreta;

namespace System.Security.Claims
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetSubjectId(this ClaimsPrincipal principal)
        {
            var claim = principal
                .FindFirst(VolvoretaClaims.Subject);

            if (claim == null)
            {
                throw new InvalidOperationException("sub claim is missing");
            }
            
            return claim.Value;
        }

        public static IEnumerable<string> GetClaimRoleValues(this ClaimsPrincipal principal, string roleClaimType)
        {
            return principal.FindAll(roleClaimType)
                .Select(x => x.Value);
        }
    }
}
