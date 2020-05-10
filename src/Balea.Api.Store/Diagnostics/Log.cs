using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;

namespace Balea.Server.Diagnostics
{
    static class Log
    {
        public static void ClientAuthorizationRequestBegin(ILogger logger, HttpRequestMessage request)
        {
            _clientAuthorizationRequestBegin(logger, request, null);
        }

        public static void ClientAuthorizationRequestEnd(ILogger logger, HttpResponseMessage response)
        {
            _clientAuthorizationRequestEnd(logger, response, null);
        }

        internal static void CacheHit(ILogger logger, string key)
        {
            cacheHit(logger, key, null);
        }

        internal static void CacheMiss(ILogger logger, string key)
        {
            cacheMiss(logger, key, null);
        }

        private static readonly Action<ILogger, HttpRequestMessage, Exception> _clientAuthorizationRequestBegin = LoggerMessage.Define<HttpRequestMessage>(
            LogLevel.Debug,
            EventIds.ClientAuthorizationRequest,
            "Balea client authorization request: {request}.");

        private static readonly Action<ILogger, HttpResponseMessage, Exception> _clientAuthorizationRequestEnd = LoggerMessage.Define<HttpResponseMessage>(
            LogLevel.Debug,
            EventIds.ClientAuthorizationResponse,
            "Balea client authorization response: {response}.");

        private static readonly Action<ILogger, string, Exception> cacheHit = LoggerMessage.Define<string>(
            LogLevel.Debug,
            EventIds.CacheHit,
            "Cache hit '{key}'");

        private static readonly Action<ILogger, string, Exception> cacheMiss = LoggerMessage.Define<string>(
            LogLevel.Information,
            EventIds.CacheMiss,
            "Cache miss '{key}'");
    }
}
