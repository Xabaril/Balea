using Balea.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Primitives;
using System;
using System.Threading.Tasks;

namespace Balea.Mvc
{
    public class BaleaTagHelper : TagHelper
    {
        public const string ROLES_NAME_ATTRIBUTE = "roles";
        public const string PERMISSIONS_NAME_ATTRIBUTE = "permissions";

        private static readonly char[] Separator = new[] { ',' };

        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IRuntimeAuthorizationServerStore store;
        private readonly IPermissionEvaluator permissionEvaluator;

        public BaleaTagHelper(
            IHttpContextAccessor httpContextAccessor,
            IRuntimeAuthorizationServerStore store,
            IPermissionEvaluator permissionEvaluator)
        {
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this.store = store ?? throw new ArgumentNullException(nameof(store));
            this.permissionEvaluator = permissionEvaluator ?? throw new ArgumentNullException(nameof(permissionEvaluator));
        }

        [HtmlAttributeName(ROLES_NAME_ATTRIBUTE)]
        public string Roles { get; set; }

        [HtmlAttributeName(PERMISSIONS_NAME_ATTRIBUTE)]
        public string Permissions { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = null;
            var authorized = false;

            if (String.IsNullOrWhiteSpace(Roles) 
                && String.IsNullOrWhiteSpace(Permissions))
            {
                return;
            }

            if (!String.IsNullOrWhiteSpace(Roles))
            {
                var roles = new StringTokenizer(Roles, Separator);

                foreach (var item in roles)
                {
                    var role = item.Trim();

                    if (role.HasValue && role.Length > 0)
                    {
                        authorized = httpContextAccessor.HttpContext.User.IsInRole(role.Value);

                        if (authorized)
                        {
                            break;
                        }
                    }
                }
            }

            if (!String.IsNullOrWhiteSpace(Permissions))
            {
                var permissions = new StringTokenizer(Permissions, Separator);

                foreach (var item in permissions)
                {
                    var permission = item.Trim();

                    if (permission.HasValue && permission.Length > 0)
                    {
                        authorized = await permissionEvaluator.HasPermissionAsync(
                            httpContextAccessor.HttpContext.User,
                            permission.Value);

                        if (authorized)
                        {
                            break;
                        }
                    }
                }
            }

            if (!authorized)
            {
                output.SuppressOutput();
            }
        }
    }
}
