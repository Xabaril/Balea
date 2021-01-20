using Antlr4.Runtime.Misc;
using System;
using System.Globalization;
using System.Linq.Expressions;
using static Balea.DSL.Grammar.Bal.BalParser;

[assembly: CLSCompliant(false)]

namespace Balea.DSL.Grammar.Bal
{
    public class BalVisitor
        : BalBaseVisitor<DslAuthorizationPolicy>
    {
        const int LEFT = 0;
        const int RIGHT = 1;

        public override DslAuthorizationPolicy VisitPolicy([NotNull] BalParser.PolicyContext context)
        {
            var policy = new DslAuthorizationPolicy(context.ID().Symbol.Text);

            foreach (var ruleItem in context.pol_rule())
            {
                Expression expression = null;

                var ruleName = FormatName(ruleItem.ID().Symbol.Text);
                var isDenyAction = ruleItem.action_id().GetText() switch
                {
                    "PERMIT" => false,
                    "DENY" => true,
                    _ => throw new ArgumentNullException($"The action identifier is not allowed to be parsed on {typeof(BalVisitor).Name} visitor.")
                };

                var parameter = Expression.Parameter(typeof(DslAuthorizationContext));
                var rule = new DslAuthorizationRule(ruleName, isDenyAction);

                foreach (var ruleCondition in ruleItem.condition())
                {
                    if (ruleCondition.bool_op() is not null)
                    {
                        expression = ParseBooleanExpression(parameter, ruleCondition);
                    }
                    else if (ruleCondition.str_comp() is not null)
                    {
                        expression = ParseStringComparasionExpression(parameter, ruleCondition);
                    }
                    else if (ruleCondition.arit_comp() is not null )
                    {
                        expression = ParseAritmeticComparisonExpression(parameter, ruleCondition);
                    }
                }

                rule.SetExpression(Expression.Lambda<Func<DslAuthorizationContext, bool>>(expression, parameter));

                policy.AddRule(rule);
            }

            return policy;
        }

        Expression ParseBooleanExpression(ParameterExpression expression, ConditionContext logicOperation)
        {
            const string and = nameof(and);

            Expression left = null;

            var boolCondition = logicOperation.bool_op().GetText();
            var binder = boolCondition.Equals(and, StringComparison.InvariantCultureIgnoreCase) ? Expression.And : (Binder)Expression.Or;

            foreach (var condition in logicOperation.condition())
            {
                if (condition.str_comp() is not null)
                {
                    var right = ParseStringComparasionExpression(expression, condition);
                    left = __Bind(left, right);
                }
                else if (condition.bool_op() is not null)
                {
                    var right = ParseBooleanExpression(expression, condition);
                    left = __Bind(left, right);
                }
                else if (condition.arit_comp() is not null)
                {
                    var right = ParseAritmeticComparisonExpression(expression, condition);
                    left = __Bind(left, right);
                }
            }

            return left;

            Expression __Bind(Expression left, Expression right) =>
                left == null ? right : binder(left, right);

        }

        private Expression ParseStringComparasionExpression(ParameterExpression parameterExpression, ConditionContext stringComparerOperation)
        {
            var comparison = stringComparerOperation.str_comp().GetText();

            Expression left, right;

            // --- LEFT
            left = CreatePropertyBagExpression(parameterExpression, stringComparerOperation.str_val()[LEFT].GetText());

            // --- RIGHT
            right = Expression.Constant(stringComparerOperation.str_val()[RIGHT].GetText().Replace("\"", ""));

            return comparison switch
            {
                "=" => Expression.Equal(left, right),
                "!=" => Expression.NotEqual(left, right),
                _ => throw new ArgumentException($"The comparison operator is not currently allowed to be parsed on {typeof(BalVisitor).Name} visitor.")
            };
        }

        private Expression ParseAritmeticComparisonExpression(ParameterExpression parameterExpression, ConditionContext aritmeticComparerOperation)
        {
            var aritmeticOperator = aritmeticComparerOperation.arit_comp().GetText();

            Expression left, right;

            // --- LEFT 

            if (aritmeticComparerOperation.arit_val()[LEFT].arit_op() is not null)
            {
                //left expression on comparison is like Subject.Id * 100 > 1000
                left = ParseAritmeticValueOperationExpression(parameterExpression, aritmeticComparerOperation.arit_val()[LEFT]);
            }
            else
            {
                //Is a simple assignation like Subject.Id > 1000
                left = CreatePropertyBagExpression(
                    parameterExpression,
                    aritmeticComparerOperation.arit_val()[LEFT].GetText(),
                    typeof(Int32));
            }

            // --- RIGHT

            var conditionPropertyValue = aritmeticComparerOperation.arit_val()[RIGHT];

            if (conditionPropertyValue.arit_val().Length == 0)
            {
                //aritmetic comparasion with simple number Subject.Id > 10
                right = CreateNumberValueExpression(conditionPropertyValue.GetText());
            }
            else
            {
                if (conditionPropertyValue.arit_op() is not null)
                {
                    //the comparison is with a aritmetic operation ie, Subject.Id > 10 * 10
                    right = ParseAritmeticValueOperationExpression(parameterExpression, conditionPropertyValue);
                }
                else
                {
                    //the comparison is with a expression ie, Subject.Id > (10*10) or Subject.Id > (10*10*10)
                    right = ParseAritmeticValueOperationExpression(parameterExpression, conditionPropertyValue.arit_val()[0]);
                }
            }

            return aritmeticOperator switch
            {
                "<" => Expression.LessThan(left, right),
                ">" => Expression.GreaterThan(left, right),
                ">=" => Expression.GreaterThanOrEqual(left, right),
                "<=" => Expression.LessThanOrEqual(left, right),
                "=" => Expression.Equal(left, right),
                "!=" => Expression.NotEqual(left, right),
                _ => throw new ArgumentException($"The specified operator for this grammar is not allowed to be parsed on {typeof(BalVisitor).Name} visitor.")
            };
        }

        private Expression ParseAritmeticValueOperationExpression(ParameterExpression parameterExpression, Arit_valContext aritmeticValueOperationContext)
        {
            Expression left, right;

            // --- LEFT

            if (aritmeticValueOperationContext.arit_val()[LEFT].arit_op() is not null)
            {
                left = ParseAritmeticValueOperationExpression(parameterExpression, aritmeticValueOperationContext.arit_val()[LEFT]);
            }
            else if (aritmeticValueOperationContext.arit_val()[LEFT].categ_attr() is not null)
            {
                left = CreatePropertyBagExpression(parameterExpression, aritmeticValueOperationContext.arit_val()[LEFT].categ_attr().GetText(), typeof(Int32));
            }
            else
            {
                left = CreateNumberValueExpression(aritmeticValueOperationContext.arit_val()[LEFT].GetText());
            }

            // --- RIGHT

            if (aritmeticValueOperationContext.arit_val()[RIGHT].arit_op() is not null)
            {
                right = ParseAritmeticValueOperationExpression(parameterExpression, aritmeticValueOperationContext.arit_val()[RIGHT]);
            }
            else
            {
                right = CreateNumberValueExpression(aritmeticValueOperationContext.arit_val()[RIGHT].GetText());
            }

            return aritmeticValueOperationContext.arit_op().GetText() switch
            {
                "*" => Expression.Multiply(left, right),
                "+" => Expression.Add(left, right),
                "-" => Expression.Subtract(left, right),
                "%" => Expression.Modulo(left, right),
                "/" => Expression.Divide(left, right),
                _ => throw new ArgumentException($"The specified aritmetic operator for this grammar is not allowed to be parsed on {typeof(BalVisitor).Name} visitor.")
            };
        }

        private Expression CreatePropertyBagExpression(ParameterExpression parameterExpression, string propertyAccessor, Type conversionType = null)
        {
            var propertyNameTokens = propertyAccessor.Split('.');

            var bag = FormatName(propertyNameTokens[LEFT]);
            var property = FormatName(propertyNameTokens[RIGHT]);

            if (conversionType != null)
            {
                return Expression.Convert(
                    Expression.Property(Expression.Property(parameterExpression, bag), "Item", Expression.Constant(property)),
                    conversionType);
            }
            else
            {
                return Expression.Property(Expression.Property(parameterExpression, bag), "Item", Expression.Constant(property));
            }
        }

        private Expression CreateNumberValueExpression(string number)
        {
            if (number == null)
            {
                throw new ArgumentNullException(nameof(number));
            }

            return Expression.Constant(Convert.ToInt32(number), typeof(Int32));
        }

        private string FormatName(string propertyName) =>
            CultureInfo.InvariantCulture
                .TextInfo
                .ToTitleCase(propertyName);

        private delegate Expression Binder(Expression left, Expression right);
    }
}
