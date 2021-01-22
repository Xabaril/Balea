using Balea.DSL.Grammar;
using Balea.DSL.Parsers;
using System;
using System.Collections.Generic;

namespace Balea.DSL
{
    /// <summary>
    /// This is the representation on code of the DSL language and allow
    /// to check if the policy is satisfied depending on <see cref="DslAuthorizationContext"/> used.
    /// </summary>
    public class DslAuthorizationPolicy
    {
        private List<DslAuthorizationRule> _authorizationRules = new List<DslAuthorizationRule>();

        /// <summary>
        /// Get the policy name.
        /// </summary>
        public string PolicyName { get; private set; }

        internal DslAuthorizationPolicy(string policyName)
        {
            PolicyName = policyName ?? throw new ArgumentNullException(nameof(policyName));
        }

        /// <summary>
        /// Check if the current policy is satisfied.
        /// </summary>
        /// <param name="dslAuthorizationContext">The current <see cref="DslAuthorizationContext"/>.</param>
        /// <returns></returns>
        public bool IsSatisfied(DslAuthorizationContext dslAuthorizationContext)
        {
            if (dslAuthorizationContext == null)
            {
                throw new ArgumentNullException(nameof(dslAuthorizationContext));
            }

            bool isSatisfied = true;

            foreach (var rule in _authorizationRules)
            {
                //evaluate all rules in the policy, checking if is a deny rule
                isSatisfied = isSatisfied && !(rule.Evaluate(dslAuthorizationContext) ^ !rule.IsDenyRule);
            }

            return isSatisfied;
        }

        internal void AddRule(DslAuthorizationRule rule)
        {
            if (rule == null)
            {
                throw new ArgumentNullException(nameof(rule));
            }

            _authorizationRules.Add(rule);
        }

        /// <summary>
        /// Create a <see cref="DslAuthorizationPolicy"/> using a specified grammar.
        /// </summary>
        /// <param name="policy">The policy.</param>
        /// <param name="grammar">The grammar to be used.</param>
        /// <returns>A <see cref="DslAuthorizationPolicy"/> created.</returns>
        public static DslAuthorizationPolicy CreateFromGrammar(string policy, AllowedGrammars grammar = AllowedGrammars.Bal)
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
