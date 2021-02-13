using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Balea.Diagnostics
{
    internal static class Log
    {
        public static void AuthorizationPolicyFound(this ILogger logger, string policyName)
        {
            _authorizationPolicyFound(logger, policyName, null);
        }

        public static void AuthorizationPolicyNotFound(this ILogger logger, string policyName)
        {
            _authorizationPolicyNotFound(logger, policyName, null);
        }

        public static void CreatingAuthorizationPolicy(this ILogger logger, string policyName)
        {
            _creatingAuthorizationPolicy(logger, policyName, null);
        }

        public static void CreatingAuthorizationPolicy(this ILogger logger, string policyName, IEnumerable<string> schemes)
        {
            _creatingAuthorizationPolicyForSchemes(logger, policyName, schemes, null);
        }

        public static void BaleaRolesFoundForUser(this ILogger logger, string user, IEnumerable<string> roles)
        {
            _baleaRolesFoundForUser(logger, user, roles, null);
        }

        public static void NoBaleaRolesForUser(this ILogger logger, string user)
        {
            _noBaleaRolesForUser(logger, user, null);
        }

        public static void ExecutingBaleaUnauthorizedFallback(this ILogger logger)
        {
            _executingBaleaUnauthorizedFallback(logger, null);
        }

        public static void NoBaleaRolesForUserAndNoUnauthorizedFallback(this ILogger logger)
        {
            _noBaleaRolesForUserAndNoUnauthorizedFallback(logger, null);
        }

        public static void PolicySucceed(this ILogger logger)
        {
            _policySucceed(logger, null);
        }

        public static void PolicyFailToForbid(this ILogger logger)
        {
            _policyFail(logger, "Forbid", null);
        }

        public static void PolicyFailToChallenge(this ILogger logger)
        {
            _policyFail(logger, "Challenge", null);
        }

        public static void PopulatePropertyBag(this ILogger logger, string propertyBag)
        {
            _populatePropertyBag(logger, propertyBag, null);
        }

        public static void PropertyBagCantBePopulated(this ILogger logger, string propertyBag)
        {
            _propertyBagCantBePopulated(logger, propertyBag, null);
        }

        public static void AbacAuthorizationHandlerThrow(this ILogger logger, Exception exception)
        {
            _abacAuthorizationHandlerThrow(logger, exception);
        }

        private static readonly Action<ILogger, string, Exception> _populatePropertyBag = LoggerMessage.Define<string>(
            LogLevel.Information,
            EventIds.PropertyBag,
            "Populating property bag {bag}.");


        private static readonly Action<ILogger, string, Exception> _propertyBagCantBePopulated = LoggerMessage.Define<string>(
            LogLevel.Warning,
            EventIds.PropertyBagCantBePopulated,
            "Populating property bag {bag} from authorization filter context.");

        private static readonly Action<ILogger, string, Exception> _authorizationPolicyFound = LoggerMessage.Define<string>(
            logLevel: LogLevel.Debug,
            eventId: EventIds.AuthorizationPolicyFound,
            formatString: "Found stored authorization policy: {policyName}.");

        private static readonly Action<ILogger, string, Exception> _authorizationPolicyNotFound = LoggerMessage.Define<string>(
            logLevel: LogLevel.Debug,
            eventId: EventIds.AuthorizationPolicyNotFound,
            formatString: "Authorization policy {policyName} not found.");

        private static readonly Action<ILogger, string, Exception> _creatingAuthorizationPolicy = LoggerMessage.Define<string>(
            logLevel: LogLevel.Debug,
            eventId: EventIds.CreatingAuthorizationPolicy,
            formatString: "Creating authorization policy {policyName} for default scheme.");

        private static readonly Action<ILogger, string, IEnumerable<string>, Exception> _creatingAuthorizationPolicyForSchemes = LoggerMessage.Define<string, IEnumerable<string>>(
            logLevel: LogLevel.Debug,
            eventId: EventIds.CreatingAuthorizationPolicy,
            formatString: "Creating authorization policy {policyName} for schemes {schemes}.");

        private static readonly Action<ILogger, string, IEnumerable<string>, Exception> _baleaRolesFoundForUser = LoggerMessage.Define<string, IEnumerable<string>>(
            logLevel: LogLevel.Debug,
            eventId: EventIds.NoBaleaRolesForUser,
            formatString: "Balea roles found for user {user}: {roles}");

        private static readonly Action<ILogger, string, Exception> _noBaleaRolesForUser = LoggerMessage.Define<string>(
            logLevel: LogLevel.Debug,
            eventId: EventIds.NoBaleaRolesForUser,
            formatString: "No Balea roles found for user {user}.");

        private static readonly Action<ILogger, Exception> _executingBaleaUnauthorizedFallback = LoggerMessage.Define(
            logLevel: LogLevel.Debug,
            eventId: EventIds.ExecutingBaleaUnauthorizedFallback,
            formatString: "Executing Balea unauthorized fallback.");

        private static readonly Action<ILogger, Exception> _noBaleaRolesForUserAndNoUnauthorizedFallback = LoggerMessage.Define(
            logLevel: LogLevel.Information,
            eventId: EventIds.NoBaleaRolesForUserAndNoUnauthorizedFallback,
            formatString: "No Balea roles found for the current user and no unauthorized fallback defined. Authorization behavior may be unexpected.");

        private static readonly Action<ILogger, Exception> _policySucceed = LoggerMessage.Define(
            logLevel: LogLevel.Debug,
            eventId: EventIds.PolicySucceed,
            formatString: "Policy succeed");

        private static readonly Action<ILogger, string, Exception> _policyFail = LoggerMessage.Define<string>(
            logLevel: LogLevel.Debug,
            eventId: EventIds.PolicyFail,
            formatString: "Policy failed. {policyResult}");

        private static readonly Action<ILogger, Exception> _abacAuthorizationHandlerThrow = LoggerMessage.Define(
           logLevel: LogLevel.Error,
           eventId: EventIds.AbacAuthorizationHandlerThrow,
           formatString: "The ABAC authorization handler throw!.");
    }
}
