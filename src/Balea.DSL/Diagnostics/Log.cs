using Microsoft.Extensions.Logging;
using System;

namespace Balea.DSL.Diagnostics
{
    static class Log
    {
        public static void PopulateUserPropertyBag(this ILogger logger, string propertyBag)
        {
            _claimsPopulatePropertyBag(logger, propertyBag, null);
        }

        public static void PopulateEndpointPropertyBag(this ILogger logger, string propertyBag)
        {
            _endpointPopulatePropertyBag(logger, propertyBag, null);
        }

        public static void PopulateAuthorizationFilterContextPropertyBag(this ILogger logger, string propertyBag)
        {
            _authorizationFilterContextPopulatePropertyBag(logger, propertyBag, null);
        }

        private static readonly Action<ILogger, string, Exception> _claimsPopulatePropertyBag = LoggerMessage.Define<string>(
            LogLevel.Debug,
            EventIds.UserPropertyBag,
            "Populating property bag {bag} from user claims.");

        private static readonly Action<ILogger, string, Exception> _endpointPopulatePropertyBag = LoggerMessage.Define<string>(
            LogLevel.Debug,
            EventIds.EndpointPropertyBag,
            "Populating property bag {bag} from endpoint.");

        private static readonly Action<ILogger, string, Exception> _authorizationFilterContextPopulatePropertyBag = LoggerMessage.Define<string>(
            LogLevel.Debug,
            EventIds.AuthorizationFilterContextPropertyBag,
            "Populating property bag {bag} from authorization filter context.");

    }
}
