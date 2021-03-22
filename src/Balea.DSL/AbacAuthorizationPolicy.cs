using Balea.DSL.Grammar;
using Balea.DSL.Parsers;
using System;
using System.Collections.Generic;

namespace Balea.DSL
{
    /// <summary>
    /// This is the representation on code of the DSL language and allow
    /// to check if the policy is satisfied depending on <see cref="AbacAuthorizationContext"/> used.
    /// </summary>
    public class AbacAuthorizationPolicy
    {
        private readonly List<AbacAuthorizationRule> _authorizationRules = new List<AbacAuthorizationRule>();

        /// <summary>
        /// Get the policy name.
        /// </summary>
        public string PolicyName { get; private set; }

        internal AbacAuthorizationPolicy(string policyName)
        {
            PolicyName = policyName ?? throw new ArgumentNullException(nameof(policyName));
        }

        /// <summary>
        /// Check if the current policy is satisfied.
        /// </summary>
        /// <param name="abacAuthorizationContext">The current <see cref="AbacAuthorizationContext"/>.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Create a <see cref="AbacAuthorizationPolicy"/> using a specified grammar.
        /// </summary>
        /// <param name="policy">The policy.</param>
        /// <param name="grammar">The grammar to be used.</param>
        /// <returns>A <see cref="AbacAuthorizationPolicy"/> created.</returns>
        public static AbacAuthorizationPolicy CreateFromGrammar(string policy, AllowedGrammars grammar = AllowedGrammars.Bal)
        {
            try
            {
                return DSLParser.Parse(policy, grammar);
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException($"Policy can't be parsed using the  grammar {Enum.GetName(typeof(AllowedGrammars), grammar)} and policy is not created succcesfully.", exception);
            }
        }
    }
}
