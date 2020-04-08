using Balea.Abstractions;
using Balea.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net.Mime;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace Balea.Endpoints
{
    public class BaleaMiddleware
    {
        private static readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        const string DEFAULT_MIME_TYPE = MediaTypeNames.Application.Json;
        const string AuthorizationMiddlewareInvokedKey = "__AuthorizationMiddlewareWithEndpointInvoked";

        private readonly RequestDelegate _next;

        public BaleaMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(
            HttpContext context, 
            IRuntimeAuthorizationServerStore store, 
            BaleaOptions options,
            ILogger<BaleaMiddleware> logger)
        {
            var endpoint = context.GetEndpoint();

            try
            {
                if (context.User.Identity.IsAuthenticated && endpoint?.Metadata?.GetMetadata<IAuthorizeData>() != null)
                {
                    if (context.Items.ContainsKey(AuthorizationMiddlewareInvokedKey))
                    {
                        ThrowMissingAuthMiddlewareException();
                    }

                    var authorization = await store
                        .FindAuthorizationAsync(context.User);

                    if (!context.Response.HasStarted && options.UnauthorizedFallback != null && !authorization.Roles.Any())
                    {
                        await options.UnauthorizedFallback(context);

                        return;
                    }

                    var roleClaims = authorization.Roles
                        .Where(role => role.Enabled)
                        .Select(role => new Claim(options.DefaultClaimTypeMap.RoleClaimType, role.Name));

                    var permissionClaims = authorization.Roles
                        .SelectMany(role => role.GetPermissions())
                        .Distinct()
                        .Select(permission => new Claim(options.DefaultClaimTypeMap.PermissionClaimType, permission));

                    var identity = new ClaimsIdentity(
                        authenticationType: nameof(BaleaMiddleware),
                        nameType: options.DefaultClaimTypeMap.NameClaimType,
                        roleType: options.DefaultClaimTypeMap.RoleClaimType);

                    identity.AddClaims(roleClaims);
                    identity.AddClaims(permissionClaims);

                    if (authorization.Delegation != null)
                    {
                        Log.BaleaMiddlewareActiveDelegation(logger, context.User.GetSubjectId(options), authorization.Delegation.Who);

                        identity.AddClaim(new Claim(BaleaClaims.DelegatedBy, authorization.Delegation.Who));
                        identity.AddClaim(new Claim(BaleaClaims.DelegatedFrom, authorization.Delegation.From.ToString()));
                        identity.AddClaim(new Claim(BaleaClaims.DelegatedTo, authorization.Delegation.To.ToString()));
                    }

                    context.User.AddIdentity(identity);

                    Log.BaleaMiddlewareAuthorizationSuccess(logger, context.User.GetSubjectId(options));
                }
            }
            catch (Exception exception)
            {
                Log.BaleaMiddlewareThrow(logger, exception);

                await WriteError(context);

                return;
            }

            await _next(context);
        }

        private static void ThrowMissingAuthMiddlewareException()
        {
            throw new InvalidOperationException("The call to app.UseAuthorization() must appear after app.UseBalea().");
        }

        private async Task WriteError(HttpContext currentContext)
        {
            await WriteAsync(
                currentContext,
                JsonSerializer.Serialize(EvaluationError.Default(), options: _serializerOptions),
                DEFAULT_MIME_TYPE,
                StatusCodes.Status500InternalServerError);
        }

        private async Task WriteAsync(
           HttpContext context,
           string content,
           string contentType,
           int statusCode)
        {
            context.Response.Headers["Content-Type"] = new[] { contentType };
            context.Response.Headers["Cache-Control"] = new[] { "no-cache, no-store, must-revalidate" };
            context.Response.Headers["Pragma"] = new[] { "no-cache" };
            context.Response.Headers["Expires"] = new[] { "0" };
            context.Response.StatusCode = statusCode;

            await context.Response.WriteAsync(content);
        }

        private class EvaluationError
        {
            public string Message { get; set; }

            private EvaluationError() { }

            public static EvaluationError Default()
            {
                return new EvaluationError()
                {
                    Message = $"Balea middleware throws an exception when check the evalution. Please review log files for further information on the error."
                };
            }
        }

    }
}
