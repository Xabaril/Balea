using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Balea.Authorization.Abac.Context
{
    internal class AbacAuthorizationRule
    {
        private Func<AbacAuthorizationContext, bool> _ruleExpression;

        public string RuleName { get; private set; }

        public bool IsDenyRule { get; private set; }

        public AbacAuthorizationRule(string ruleName, bool isDenyRule = false)
        {
            Ensure.NotNull(ruleName);

            RuleName = ruleName;
            IsDenyRule = isDenyRule;
        }

        internal void SetRuleExpression(Expression<Func<AbacAuthorizationContext, bool>> expression)
        {
            if (expression != null)
            {
                if (expression.CanReduce)
                {
                    expression.ReduceAndCheck();
                }

                _ruleExpression = expression.Compile();
            }
        }

        internal bool Evaluate(AbacAuthorizationContext context)
        {
            try
            {
                return _ruleExpression(context);
            }
            catch (KeyNotFoundException keyNotFoundException)
            {
                //evaluating a expression that use a property that does not exist on context bag's
                throw new InvalidOperationException($"The rule {RuleName} is evaluating a property that does not exist on actual DslAuthorizationContext", keyNotFoundException);
            }
            catch (Exception exception)
            {
                //other exception out of scope
                throw new InvalidOperationException($"The rule {RuleName} is not evaluated succesfully.", exception);
            }
        }
    }
}
