using Microsoft.Extensions.Logging;
using System;

namespace Balea.Diagnostics
{
    static class Log
    {
        public static void BaleaMiddlewareThrow(ILogger logger, Exception exception)
        {
            _baleaMiddlewareThrow(logger, exception);
        }

        public static void BaleaMiddlewareAuthorizationSuccess(ILogger logger, string id)
        {
            _baleaMiddlewareAuthorizationSuccess(logger, id, null);
        }

        public static void BaleaMiddlewareActiveDelegation(ILogger logger, string id, string delegationId)
        {
            _baleaMiddlewareActiveDelegation(logger, id, delegationId, null);
        }

        private static readonly Action<ILogger, Exception> _baleaMiddlewareThrow = LoggerMessage.Define(
            LogLevel.Error,
            EventIds.BaleaMiddlewareThrow,
            "Balea middleware throw exception when evaluating authorization.");

        private static readonly Action<ILogger, string, Exception> _baleaMiddlewareAuthorizationSuccess = LoggerMessage.Define<string>(
            LogLevel.Information,
            EventIds.BaleaMiddlewareAuthorizationSuccess,
            "Balea middleware perform authorization evaluation succesfully for subject/client '{id}'.");

        private static readonly Action<ILogger, string, string, Exception> _baleaMiddlewareActiveDelegation = LoggerMessage.Define<string,string>(
            LogLevel.Information,
            EventIds.BaleaMiddlewareActiveDelegation,
            "Subject/client '{id}' is acting on behalf of subject/client {delegationId}.");
    }
}
