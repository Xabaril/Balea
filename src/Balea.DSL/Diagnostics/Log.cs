using Microsoft.Extensions.Logging;
using System;

namespace Balea.DSL.Diagnostics
{
    static class Log
    {
        public static void PopulatePropertyBag(this ILogger logger, string propertyBag)
        {
            _populatePropertyBag(logger, propertyBag, null);
        }

        public static void PropertyBagCantBePopulated(this ILogger logger, string propertyBag)
        {
            _propertyBagCantBePopulated(logger, propertyBag, null);
        }

        private static readonly Action<ILogger, string, Exception> _populatePropertyBag = LoggerMessage.Define<string>(
            LogLevel.Information,
            EventIds.PropertyBag,
            "Populating property bag {bag}.");


        private static readonly Action<ILogger, string, Exception> _propertyBagCantBePopulated = LoggerMessage.Define<string>(
            LogLevel.Warning,
            EventIds.PropertyBagCantBePopulated,
            "Populating property bag {bag} from authorization filter context.");

    }
}
