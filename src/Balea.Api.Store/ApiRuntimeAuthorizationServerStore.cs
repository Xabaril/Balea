using Balea.Abstractions;
using Balea.Api.Store.Model;
using Balea.Api.Store.Options;
using Balea.Model;
using Balea.Server.Diagnostics;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Balea.Api.Store
{
    public class ApiRuntimeAuthorizationServerStore : IRuntimeAuthorizationServerStore
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly StoreOptions _storeOptions;
        private readonly BaleaOptions _baleaOptions;
        private readonly ILogger<ApiRuntimeAuthorizationServerStore> _logger;
        private readonly IDistributedCache _cache;

        public ApiRuntimeAuthorizationServerStore(
            IHttpClientFactory httpClientFactory,
            StoreOptions storeOptions,
            BaleaOptions baleaOptions,
            ILogger<ApiRuntimeAuthorizationServerStore> logger,
            IDistributedCache cache = null)
        {
            Ensure.Argument.NotNull(httpClientFactory, nameof(httpClientFactory));
            Ensure.Argument.NotNull(storeOptions, nameof(storeOptions));
            Ensure.Argument.NotNull(baleaOptions, nameof(baleaOptions));
            Ensure.Argument.NotNull(logger, nameof(logger));
            _httpClientFactory = httpClientFactory;
            _storeOptions = storeOptions;
            _baleaOptions = baleaOptions;
            _logger = logger;
            _cache = cache;
        }

        public async Task<AuthorizationContext> FindAuthorizationAsync(ClaimsPrincipal user, CancellationToken cancellationToken = default)
        {
            if (_storeOptions.CacheEnabled)
            {
                var key = $"balea:1.0:user:{user.GetSubjectId(_baleaOptions)}:application:{_baleaOptions.ApplicationName}";
                
                var cachedResponse = await _cache.GetOrSet(
                    key,
                    miss: () => 
                    { 
                        Log.CacheMiss(_logger, key); 
                        return GetAuthorizationContext(user);
                    },
                    hit: _ => Log.CacheHit(_logger, key),
                    _storeOptions.AbsoluteExpirationRelativeToNow,
                    _storeOptions.SlidingExpiration,
                    cancellationToken);

                return cachedResponse.To();
            }

            var response = await GetAuthorizationContext(user);

            return response.To();
        }

        public async Task<Policy> GetPolicyAsync(string name, CancellationToken cancellationToken = default)
        {
            if (_storeOptions.CacheEnabled)
            {
                var key = $"balea:1.0:application:{_baleaOptions.ApplicationName}:policy:{name}";

                var cachedResponse = await _cache.GetOrSet(
                    key,
                    miss: () =>
                    {
                        Log.CacheMiss(_logger, key);
                        return GetPolicy(name);
                    },
                    hit: _ => Log.CacheHit(_logger, key),
                    _storeOptions.AbsoluteExpirationRelativeToNow,
                    _storeOptions.SlidingExpiration,
                    cancellationToken);

                return cachedResponse.To();
            }

            var response = await GetPolicy(name);

            return response.To();
        }

        private async Task<HttpClientStoreAuthorizationResponse> GetAuthorizationContext(ClaimsPrincipal user)
        {
            var client = _httpClientFactory.CreateClient(Constants.BaleaClient);

            return await client.GetJsonAsync<HttpClientStoreAuthorizationResponse>(
                $"api/users/{user.GetSubjectId(_baleaOptions)}/applications/{_baleaOptions.ApplicationName}/authorization?api-version=1.0{GetMappings(user)}");
        }

        private async Task<HttpClientStorePolicyResponse> GetPolicy(string name)
        {
            var client = _httpClientFactory.CreateClient(Constants.BaleaClient);

            return await client.GetJsonAsync<HttpClientStorePolicyResponse>(
                $"api/applications/{_baleaOptions.ApplicationName}/policies/{name}?api-version=1.0");
        }

        private string GetMappings(ClaimsPrincipal user)
        {
            return string.Concat(user
                .GetClaimValues(_baleaOptions.DefaultClaimTypeMap.RoleClaimType)
                .Select(role => $"&roles={role}"));
        }
    }
}
