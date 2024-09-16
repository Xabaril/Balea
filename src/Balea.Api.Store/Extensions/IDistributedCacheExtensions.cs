using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Caching.Distributed
{
    public static class DistributedCacheExtensions
    {
        public static readonly JsonSerializerOptions _serializationOptions = new JsonSerializerOptions()
        {
            AllowTrailingCommas = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
#if NET5_0_OR_GREATER
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
#else
            IgnoreNullValues = false,
#endif
            ReadCommentHandling = JsonCommentHandling.Disallow
        };

        public static async Task<T> Get<T>(this IDistributedCache cache, string key, CancellationToken token = default)
        {
            var json = await cache.GetStringAsync(key, token);

            if (string.IsNullOrEmpty(json))
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(json, _serializationOptions);
        }

        public static async Task<T> GetOrSet<T>(
            this IDistributedCache cache, string key,
            Func<Task<T>> result,
            TimeSpan? expires = null,
            TimeSpan? slidingExpiration = null,
            CancellationToken token = default,
            Func<T, TimeSpan?, CancellationToken, Task<T>> setter = null)
        {
            var data = await cache.Get<T>(key, token);

            if (data != null)
            {
                return data;
            }

            data = await result();

            if (data == null)
            {
                return data;
            }

            if (setter != null)
            {
                data = await setter(data, expires, token);
            }
            else
            {
                await cache.Insert(key, data, expires, slidingExpiration, token);
            }

            return data;
        }
        public static async Task<T> GetOrSet<T>(
            this IDistributedCache cache, string key,
            Func<Task<T>> miss,
            Action<T> hit,
            TimeSpan? expires = null,
            TimeSpan? slidingExpiration = null,
            CancellationToken token = default)
        {
            var data = await cache.Get<T>(key, token);

            if (data != null)
            {
                hit(data);
                return data;
            }

            data = await miss();

            await cache.Insert(key, data, expires, slidingExpiration, token);

            return data;
        }


        public static Task Insert<T>(this IDistributedCache cache, string key, T data, TimeSpan? expires = null, TimeSpan? slidingExpiration = null, CancellationToken token = default)
        {
            return cache.SetStringAsync(
                key,
                JsonSerializer.Serialize(data, _serializationOptions),
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = expires, SlidingExpiration = slidingExpiration });
        }

        public static Task Remove(this IDistributedCache cache, string key, CancellationToken token = default)
        {
            return cache.RemoveAsync(key, token);
        }

        public static Task Refresh(this IDistributedCache cache, string key, CancellationToken token = default)
        {
            return cache.RefreshAsync(key, token);
        }
    }
}
