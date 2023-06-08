using System;
using System.Net.Http.Headers;

namespace Balea.Api.Store.Options
{
    public class StoreOptions
    {
        internal bool CacheEnabled = false;
        internal TimeSpan? AbsoluteExpirationRelativeToNow = null;
        internal TimeSpan? SlidingExpiration = null;
        internal int RetryCount = 3;
        internal int HandledEventsAllowedBeforeBreaking = 5;
        internal int DurationOfBreak = 30;
        internal string ApiKey;
        internal Uri BaseAddress;
        internal HttpRequestHeaders Headers;

        /// <summary>
        /// Set the base addres for the Balea Server.
        /// </summary>
        /// <param name="uri">The base addres for the Balea Server</param>
        /// <returns>StoreOptions</returns>
        public StoreOptions UseBaseAddress(Uri uri)
        {
            BaseAddress = uri;
            return this;
        }

        /// <summary>
        /// Set the Api Key for access to the Balea server REST API.
        /// </summary>
        /// <param name="apiKey">The Api Key for access to the Balea server REST API</param>
        /// <returns></returns>
        public StoreOptions UseApiKey(string apiKey)
        {
            Ensure.Argument.NotNullOrEmpty(apiKey, nameof(apiKey));
            ApiKey = apiKey;
            return this;
        }

        /// <summary>
        /// Enable cache at the client level. You need to register an implementation of <see cref="IDistributedCache"/>
        /// </summary>
        /// <param name="absoluteExpirationRelativeToNow"></param>
        /// <param name="slidingExpiration"></param>
        /// <returns></returns>
        public StoreOptions UseCache(TimeSpan? absoluteExpirationRelativeToNow = null, TimeSpan? slidingExpiration = null)
        {
            CacheEnabled = true;
            AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow;
            SlidingExpiration = slidingExpiration;
            return this;
        }


        /// <summary>
        /// Set the the retry count for the client.
        /// </summary>
        /// <param name="retryCount">The retry count. Default value 3</param>
        /// <returns>StoreOptions</returns>
        public StoreOptions UseRetryCount(int retryCount)
        {
            RetryCount = retryCount;
            return this;
        }

        /// <summary>
        /// Set the number of exceptions or handled results that are allowed before opening the circuit.
        /// </summary>
        /// <param name="events">The number of exceptions or handled results that are allowed before opening the circuit. Default value 5</param>
        /// <returns>StoreOptions</returns>
        public StoreOptions UseHandledEventsAllowedBeforeBreaking(int events)
        {
            HandledEventsAllowedBeforeBreaking = events;
            return this;
        }

        /// <summary>
        /// Set the duration in seconds of the circuit will stay open before resetting
        /// </summary>
        /// <param name="durationOfBreak">The duration in seconds the circuit will stay open before resetting. Default value 30sg</param>
        /// <returns>StoreOptions</returns>
        public StoreOptions UseDurationOfBreak(int durationOfBreak)
        {
            DurationOfBreak = durationOfBreak;
            return this;
        }

        /// <summary>
        /// Set the request headers for the client.
        /// </summary>
        /// <param name="headers">A HttpRequestHeaders object with the desired headers</param>
        /// <returns>StoreOptions</returns>
        public StoreOptions UseRequestHeaders(HttpRequestHeaders headers)
        {
            Headers = headers;
            return this;
        }
    }
}
