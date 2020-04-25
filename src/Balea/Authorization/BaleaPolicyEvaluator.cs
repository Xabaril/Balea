using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Balea.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;

namespace Balea.Authorization
{
    public class BaleaPolicyEvaluator : PolicyEvaluator
    {
        private readonly IRuntimeAuthorizationServerStore _store;
        private readonly BaleaOptions _options;

        public BaleaPolicyEvaluator(
            IAuthorizationService authorization,
            IRuntimeAuthorizationServerStore store,
            BaleaOptions options)
            : base(authorization)
        {
            _store = store;
            _options = options;
        }

        public override async Task<AuthenticateResult> AuthenticateAsync(AuthorizationPolicy policy, HttpContext context)
        {
            var baleaHasSchemes = _options.Schemes.Any();

            if (policy.AuthenticationSchemes != null && policy.AuthenticationSchemes.Count > 0)
            {
                ClaimsPrincipal newPrincipal = null;
                var baleaMatchPolicySchemes = false;
                foreach (var scheme in policy.AuthenticationSchemes)
                {
                    if (_options.Schemes.Any(s => s.Equals(scheme, StringComparison.OrdinalIgnoreCase)))
                    {
                        baleaMatchPolicySchemes = true;
                    }

                    var result = await context.AuthenticateAsync(scheme);
                    if (result != null && result.Succeeded)
                    {
                        newPrincipal = SecurityHelper.MergeUserPrincipal(newPrincipal, result.Principal);
                    }
                }

                if (newPrincipal != null)
                {
                    context.User = newPrincipal;
                    
                    if (baleaMatchPolicySchemes)
                    {
                        await AddBaleaIdentity(context.User, context);
                    }

                    return AuthenticateResult.Success(new AuthenticationTicket(newPrincipal, string.Join(";", policy.AuthenticationSchemes)));
                }

                context.User = new ClaimsPrincipal(new ClaimsIdentity());
                return AuthenticateResult.NoResult();
            }

            if (context.User?.Identity?.IsAuthenticated ?? false)
            {
                // Only apply balea policies if balea is configured without specific schemes
                if (!baleaHasSchemes)
                {
                    await AddBaleaIdentity(context.User, context);
                }

                return AuthenticateResult.Success(new AuthenticationTicket(context.User, "context.User"));
            }

            return AuthenticateResult.NoResult();
        }

        private async Task AddBaleaIdentity(ClaimsPrincipal user, HttpContext context)
        {
            var authorization = await _store
                .FindAuthorizationAsync(user);

            if (!context.Response.HasStarted && _options.UnauthorizedFallback != null && !authorization.Roles.Any())
            {
                await _options.UnauthorizedFallback(context);

                return;
            }

            var roleClaims = authorization.Roles
                .Where(role => role.Enabled)
                .Select(role => new Claim(_options.DefaultClaimTypeMap.RoleClaimType, role.Name));

            var permissionClaims = authorization.Roles
                .SelectMany(role => role.GetPermissions())
                .Distinct()
                .Select(permission => new Claim(_options.DefaultClaimTypeMap.PermissionClaimType, permission));

            var identity = new ClaimsIdentity(
                authenticationType: "Balea",
                nameType: _options.DefaultClaimTypeMap.NameClaimType,
                roleType: _options.DefaultClaimTypeMap.RoleClaimType);

            identity.AddClaims(roleClaims);
            identity.AddClaims(permissionClaims);

            if (authorization.Delegation != null)
            {
                identity.AddClaim(new Claim(BaleaClaims.DelegatedBy, authorization.Delegation.Who));
                identity.AddClaim(new Claim(BaleaClaims.DelegatedFrom, authorization.Delegation.From.ToString()));
                identity.AddClaim(new Claim(BaleaClaims.DelegatedTo, authorization.Delegation.To.ToString()));
            }

            user.AddIdentity(identity);
        }
    }

    internal static class SecurityHelper
    {
        /// <summary>
        /// Add all ClaimsIdentities from an additional ClaimPrincipal to the ClaimsPrincipal
        /// Merges a new claims principal, placing all new identities first, and eliminating
        /// any empty unauthenticated identities from context.User
        /// </summary>
        /// <param name="existingPrincipal">The <see cref="ClaimsPrincipal"/> containing existing <see cref="ClaimsIdentity"/>.</param>
        /// <param name="additionalPrincipal">The <see cref="ClaimsPrincipal"/> containing <see cref="ClaimsIdentity"/> to be added.</param>
        public static ClaimsPrincipal MergeUserPrincipal(ClaimsPrincipal existingPrincipal, ClaimsPrincipal additionalPrincipal)
        {
            var newPrincipal = new ClaimsPrincipal();

            // New principal identities go first
            if (additionalPrincipal != null)
            {
                newPrincipal.AddIdentities(additionalPrincipal.Identities);
            }

            // Then add any existing non empty or authenticated identities
            if (existingPrincipal != null)
            {
                newPrincipal.AddIdentities(existingPrincipal.Identities.Where(i => i.IsAuthenticated || i.Claims.Any()));
            }
            return newPrincipal;
        }
    }
}