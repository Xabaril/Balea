using Balea.Authorization.Abac.Grammars;
using Balea.Authorization.Abac.Parsers;
using System;
using System.Collections.Generic;

namespace Balea.Authorization.Abac.Context
{
    internal class AbacAuthorizationPolicy
    {
        private readonly List<AbacAuthorizationRule> _authorizationRules = new List<AbacAuthorizationRule>();

        public string PolicyName { get; private set; }

        internal AbacAuthorizationPolicy(string policyName)
        {
            PolicyName = policyName ?? throw new ArgumentNullException(nameof(policyName));
        }

        public bool IsSatisfied(AbacAuthorizationContext abacAuthorizationContext)
        {
            if (abacAuthorizationContext == null)
            {
                throw new ArgumentNullException(nameof(abacAuthorizationContext));
            }

            bool isSatisfied = true;

            foreach (var rule in _authorizationRules)
            {
                //evaluate all rules in the policy, checking if is a deny rule
                isSatisfied = isSatisfied && !(rule.Evaluate(abacAuthorizationContext) ^ !rule.IsDenyRule);
            }

            return isSatisfied;
        }

        internal void AddRule(AbacAuthorizationRule rule)
        {
            if (rule == null)
            {
                throw new ArgumentNullException(nameof(rule));
            }

            _authorizationRules.Add(rule);
        }

        public static AbacAuthorizationPolicy CreateFromGrammar(string policy, WellKnownGrammars grammar = WellKnownGrammars.Bal)
        {
            try
            {
                return DefaultParser.Parse(policy, grammar);
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException($"Policy can't be parsed using the  grammar {Enum.GetName(typeof(WellKnownGrammars), grammar)} and policy is not created succcesfully.", exception);
            }
        }
    }
}
