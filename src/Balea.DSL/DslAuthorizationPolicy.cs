using System;
using System.Collections.Generic;

namespace Balea.DSL
{
    public class DslAuthorizationPolicy
    {
        private List<DslAuthorizationRule> _authorizationRules = new List<DslAuthorizationRule>();

        public string PolicyName { get; private set; }

        public DslAuthorizationPolicy(string policyName)
        {
            PolicyName = policyName ?? throw new ArgumentNullException(nameof(policyName));
        }

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
    }
}
