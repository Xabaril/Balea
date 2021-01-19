using System;
using System.Linq.Expressions;

namespace Balea.DSL
{
    internal class DslAuthorizationRule
    {
        private Func<DslAuthorizationContext, bool> _expression;

        public string RuleName { get; private set; }

        public bool IsDenyRule { get; private set; }

        public DslAuthorizationRule(string ruleName, bool isDenyRule = false)
        {
            RuleName = ruleName ?? throw new ArgumentNullException(nameof(ruleName));
            IsDenyRule = isDenyRule;
        }

        internal void SetExpression(Expression<Func<DslAuthorizationContext, bool>> expression)
        {
            if (expression != null)
            {
                if (expression.CanReduce)
                {
                    expression.ReduceAndCheck();
                }

                _expression = expression.Compile();
            }
        }

        internal bool Evaluate(DslAuthorizationContext context)
        {
            return _expression(context);
        }
    }
}
