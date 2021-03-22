using Antlr4.Runtime.Misc;
using Balea.Authorization.Abac.Context;
using Balea.Authorization.Abac.Grammars.BAL;
using System;
using System.Globalization;
using System.Linq.Expressions;
using static Balea.Authorization.Abac.Grammars.BAL.BalParser;

namespace Balea.DSL.Grammar.Bal
{
    internal class BalVisitor
        : BalBaseVisitor<AbacAuthorizationPolicy>
    {
        private const int LEFT = 0;
        private const int RIGHT = 1;
        private const char ID_SEPARATOR = '.';

        public override AbacAuthorizationPolicy VisitPolicy([NotNull] BalParser.PolicyContext context)
        {
            //create the policy and assign the rules on it

            var policy = new AbacAuthorizationPolicy(
                FormatName(context.ID().Symbol.Text));

            foreach (var ruleItem in context.pol_rule())
            {
                //this create a lambda expression with the rule spec and set if rule is PERMIT or DENY rule

                ParameterExpression parameter = Expression.Parameter(typeof(AbacAuthorizationContext));
                Expression ruleExpression = null;

                var isDenyAction = ruleItem.action_id().GetText() switch
                {
                    "PERMIT" => false,
                    "DENY" => true,
                    _ => throw new ArgumentNullException($"The action identifier is not allowed to be parsed on {typeof(BalVisitor).Name} visitor.")
                };

                var rule = new AbacAuthorizationRule(FormatName(ruleItem.ID().Symbol.Text), isDenyAction);

                var ruleCondition = ruleItem.condition();

                if (ruleCondition.bool_op() is not null)
                {
                    ruleExpression = ParseBooleanExpression(parameter, ruleCondition);
                }
                else if (ruleCondition.str_comp() is not null)
                {
                    ruleExpression = ParseStringComparasionExpression(parameter, ruleCondition);
                }
                else if (ruleCondition.arit_comp() is not null)
                {
                    ruleExpression = ParseAritmeticComparisonExpression(parameter, ruleCondition);
                }

                rule.SetRuleExpression(
                    Expression.Lambda<Func<AbacAuthorizationContext, bool>>(ruleExpression, parameter));

                policy.AddRule(rule);
            }

            return policy;
        }

        Expression ParseBooleanExpression(ParameterExpression expression, ConditionContext logicOperation)
        {
            const string and = nameof(and);

            Expression left = null;

            var boolCondition = logicOperation.bool_op().GetText();
            var binder = boolCondition.Equals(and, StringComparison.InvariantCultureIgnoreCase) 
                ? Expression.And : (Binder)Expression.Or;

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
                else
                {
                    // It is a new expression like " ( some condition ) " and we need to evaluate again
                    // all inner conditions on this expression
                    var innerConditions = condition.condition();

                    foreach (var innerCondition in innerConditions)
                    {
                        if (innerCondition.str_comp() is not null)
                        {
                            var right = ParseStringComparasionExpression(expression, innerCondition);
                            left = __Bind(left, right);
                        }
                        else if (innerCondition.bool_op() is not null)
                        {
                            var right = ParseBooleanExpression(expression, innerCondition);
                            left = __Bind(left, right);
                        }
                        else if (innerCondition.arit_comp() is not null)
                        {
                            var right = ParseAritmeticComparisonExpression(expression, innerCondition);
                            left = __Bind(left, right);
                        }
                    }
                }
            }

            return left;

            Expression __Bind(Expression left, Expression right) =>
                left == null ? right : binder(left, right);
        }

        private static Expression ParseStringComparasionExpression(ParameterExpression parameterExpression, ConditionContext stringComparerOperation)
        {
            var comparison = stringComparerOperation.str_comp().GetText();
            
            if (comparison.Equals("CONTAINS",StringComparison.InvariantCultureIgnoreCase))
            {
                return CreatePropertyBagContains(parameterExpression, 
                    stringComparerOperation.str_val()[LEFT].GetText(),
                    stringComparerOperation.str_val()[RIGHT].GetText().Replace("\"", ""));
            }
            else
            {
                // --- LEFT
                var left = CreatePropertyBagExpression(parameterExpression, stringComparerOperation.str_val()[LEFT].GetText(), typeof(String));
                
                // --- right 
                var right = Expression.Constant(stringComparerOperation.str_val()[RIGHT].GetText().Replace("\"", ""));

                return comparison switch
                {
                    "=" => Expression.Equal(left, right),
                    "<>" => Expression.NotEqual(left, right),
                    "!=" => Expression.NotEqual(left, right),
                    _ => throw new ArgumentException($"The comparison operator is not currently allowed to be parsed on {typeof(BalVisitor).Name} visitor.")
                };
            }
        }

        private static Expression ParseAritmeticComparisonExpression(ParameterExpression parameterExpression, ConditionContext aritmeticComparerOperation)
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

        private static Expression ParseAritmeticValueOperationExpression(ParameterExpression parameterExpression, Arit_valContext aritmeticValueOperationContext)
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

        private static Expression CreatePropertyBagExpression(ParameterExpression parameterExpression, string propertyAccessor, Type conversionType = null)
        {
            var propertyNameTokens = propertyAccessor.Split(ID_SEPARATOR);

            var propertyBag = FormatName(propertyNameTokens[LEFT]);
            var propertyName = FormatName(propertyNameTokens[RIGHT]);

            // -> DslAuthorizationContext["PropertyBag"]
            var propertyBagExpression = Expression.Property(parameterExpression, "Item", Expression.Constant(propertyBag));

            if (conversionType != null)
            {
                // -> (conversionType) DslAuthorizationContext["propertyBag"]["propertyName"] == 
                return Expression.Convert(
                    Expression.Property(propertyBagExpression, "Item", Expression.Constant(propertyName)), conversionType);
            }
            else
            {
                // -> DslAuthorizationContext["propertyBag"]["propertyName"] == 
                return Expression.Property(propertyBagExpression, "Item", Expression.Constant(propertyName));
            }
        }

        private static Expression CreatePropertyBagContains(ParameterExpression parameterExpression, string propertyAccessor, object value)
        {
            var propertyNameTokens = propertyAccessor.Split(ID_SEPARATOR);

            var propertyBag = FormatName(propertyNameTokens[LEFT]);
            var propertyName = FormatName(propertyNameTokens[RIGHT]);

            // -> DslAuthorizationContext["PropertyBag"]
            var propertyBagExpression = Expression.Property(parameterExpression, "Item", Expression.Constant(propertyBag));

            return Expression.Call(propertyBagExpression, typeof(IPropertyBag).GetMethod("Contains"),Expression.Constant(propertyName), Expression.Constant(value));
        }

        private static Expression CreateNumberValueExpression(string number) =>
            Expression.Constant(Convert.ToInt32(number), typeof(Int32));

        private static string FormatName(string propertyName) =>
            CultureInfo.InvariantCulture
                .TextInfo
                .ToTitleCase(propertyName);

        private delegate Expression Binder(Expression left, Expression right);
    }
}
