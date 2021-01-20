﻿using Balea.DSL;
using Balea.DSL.Grammar;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace FunctionalTests.Scenarios
{
    public class bal_grammar
    {
        [Fact]
        public void visitor_allow_to_parse_and_logical_conditions()
        {
            const string policy = @"
            policy Example begin
                rule CardiologyNurses (PERMIT) begin
                    Subject.Role = ""Nurse"" AND  Resource.Action = ""MedicalRecord""
                end
            end";

            var dslAuthorizationPolicy = DslAuthorizationPolicy.CreateFromGrammar(policy, Grammars.Bal);

            dslAuthorizationPolicy.PolicyName.Should().BeEquivalentTo("Example");

            var context = new DslAuthorizationContext()
            {
                Subject = new Dictionary<string, object>()
                {
                    {"Role", "Nurse" }
                },
                Resource = new Dictionary<string, object>()
                {
                    {"Action", "MedicalRecord" }
                }
            };

            dslAuthorizationPolicy.IsSatisfied(context).Should().BeTrue();
        }

        [Fact]
        public void visitor_allow_to_parse_or_logical_conditions()
        {
            const string policy = @"
            policy Example begin
                rule CardiologyNurses (PERMIT) begin
                    Subject.Name = ""Mary Joe"" OR  Subject.Name = ""Jhon Doe""
                end
            end";

            var dslAuthorizationPolicy = DslAuthorizationPolicy.CreateFromGrammar(policy, Grammars.Bal);

            dslAuthorizationPolicy.PolicyName.Should().BeEquivalentTo("Example");

            var context = new DslAuthorizationContext()
            {
                Subject = new Dictionary<string, object>()
                {
                    {"Name", "Scott Hunter" }
                }
            };

            dslAuthorizationPolicy.IsSatisfied(context).Should().BeFalse();

            context = new DslAuthorizationContext()
            {
                Subject = new Dictionary<string, object>()
                {
                    {"Name", "Mary Joe" }
                }
            };

            dslAuthorizationPolicy.IsSatisfied(context).Should().BeTrue();
        }

        [Fact]
        public void visitor_allow_to_parse_with_not_title_case()
        {
            const string policy = @"
            policy Example begin
                rule CardiologyNurses (PERMIT) begin
                    suBjEct.roLe = ""Nurse"" AND  reSourcE.acTion = ""MedicalRecord""
                end
            end";

            var dslAuthorizationPolicy = DslAuthorizationPolicy.CreateFromGrammar(policy, Grammars.Bal);

            dslAuthorizationPolicy.PolicyName.Should().BeEquivalentTo("Example");

            var context = new DslAuthorizationContext()
            {
                Subject = new Dictionary<string, object>()
                {
                    {"Role", "Nurse" }
                },
                Resource = new Dictionary<string, object>()
                {
                    {"Action", "MedicalRecord" }
                }
            };

            dslAuthorizationPolicy.IsSatisfied(context).Should().BeTrue();
        }

        [Fact]
        public void visitor_allow_to_parse_multiple_logical_conditions()
        {
            const string policy = @"
            policy Example begin
                rule CardiologyNurses (PERMIT) begin
                    Subject.Role = ""Nurse"" 
                    AND Subject.Name = ""Mary Joe""
                    AND Resource.Action = ""MedicalRecord""
                end
            end";

            var dslAuthorizationPolicy = DslAuthorizationPolicy.CreateFromGrammar(policy, Grammars.Bal);

            dslAuthorizationPolicy.PolicyName.Should().BeEquivalentTo("Example");

            var context = new DslAuthorizationContext()
            {
                Subject = new Dictionary<string, object>()
                {
                    {"Role", "Nurse" },
                    {"Name", "Jhon Doe" },
                },
                Resource = new Dictionary<string, object>()
                {
                    {"Action", "MedicalRecord" }
                }
            };

            dslAuthorizationPolicy.IsSatisfied(context).Should().BeFalse();
        }

        [Fact]
        public void visitor_allow_to_use_rule_action()
        {
            const string policy = @"
            policy Example begin
                rule CardiologyNurses (DENY) begin
                    Subject.Role = ""Nurse"" 
                    AND Subject.Name = ""Mary Joe""
                end
            end";

            var dslAuthorizationPolicy = DslAuthorizationPolicy.CreateFromGrammar(policy, Grammars.Bal);

            dslAuthorizationPolicy.PolicyName.Should().BeEquivalentTo("Example");

            var context = new DslAuthorizationContext()
            {
                Subject = new Dictionary<string, object>()
                {
                    {"Role", "Nurse" },
                    {"Name", "Mary Joe" },
                }
            };

            dslAuthorizationPolicy.IsSatisfied(context).Should().BeFalse();
        }

        [Fact]
        public void visitor_allow_to_use_multiple_rules()
        {
            const string policy = @"
            policy Example begin
                rule CardiologyNurses (PERMIT) begin
                    Subject.Role = ""Nurse"" 
                    AND Resource.Action = ""medicalreports""
                end
                rule CardiologyNursesExcepJhonDoe (DENY) begin
                    Subject.Role = ""Nurse"" 
                    AND Resource.Action = ""medicalreports""
                    AND Subject.Name = ""Jhon Doe""
                end
            end";

            var dslAuthorizationPolicy = DslAuthorizationPolicy.CreateFromGrammar(policy, Grammars.Bal);

            dslAuthorizationPolicy.PolicyName.Should().BeEquivalentTo("Example");

            var context = new DslAuthorizationContext()
            {
                Subject = new Dictionary<string, object>()
                {
                    {"Role", "Nurse" },
                    {"Name", "Mary Joe" },
                },
                Resource = new Dictionary<string, object>()
                {
                    {"Action", "medicalreports" }
                }
            };

            dslAuthorizationPolicy.IsSatisfied(context).Should().BeTrue();

            context = new DslAuthorizationContext()
            {
                Subject = new Dictionary<string, object>()
                {
                    {"Role", "Nurse" },
                    {"Name", "Jhon Doe" },
                },
                Resource = new Dictionary<string, object>()
                {
                    {"Action", "medicalreports" }
                }
            };

            dslAuthorizationPolicy.IsSatisfied(context).Should().BeFalse();
        }

        [Fact]
        public void visitor_allow_to_parse_aritmetic_conditions()
        {
            const string policy = @"
            policy Example begin
                rule CardiologyNurses (PERMIT) begin
                    Subject.Age < 20 AND  Resource.Id > 1000
                end
            end";

            var dslAuthorizationPolicy = DslAuthorizationPolicy.CreateFromGrammar(policy, Grammars.Bal);

            dslAuthorizationPolicy.PolicyName.Should().BeEquivalentTo("Example");

            var context = new DslAuthorizationContext()
            {
                Subject = new Dictionary<string, object>()
                {
                    {"Age", 19 }
                },
                Resource = new Dictionary<string, object>()
                {
                    {"Id", 1001}
                }
            };

            dslAuthorizationPolicy.IsSatisfied(context).Should().BeTrue();
        }

        [Fact]
        public void visitor_allow_to_parse_aritmetic_operations()
        {
            const string policy = @"
            policy Example begin
                rule CardiologyNurses (PERMIT) begin
                    Subject.Role = ""Nurse"" AND  Resource.Id > (10 * 100 * 10)
                end
            end";

            var dslAuthorizationPolicy = DslAuthorizationPolicy.CreateFromGrammar(policy, Grammars.Bal);

            dslAuthorizationPolicy.PolicyName.Should().BeEquivalentTo("Example");

            var context = new DslAuthorizationContext()
            {
                Subject = new Dictionary<string, object>()
                {
                    {"Role", "Nurse" }
                },
                Resource = new Dictionary<string, object>()
                {
                    {"Id", 999}
                }
            };

            dslAuthorizationPolicy.IsSatisfied(context).Should().BeFalse();
        }

        [Fact]
        public void visitor_allow_to_parse_aritmetic_operations_with_context_data()
        {
            const string policy = @"
            policy Example begin
                rule CardiologyNurses (PERMIT) begin
                    Subject.Age < 20 AND  Subject.Id * 1000 >= 1000 * 1
                end
            end";

            var dslAuthorizationPolicy = DslAuthorizationPolicy.CreateFromGrammar(policy, Grammars.Bal);

            dslAuthorizationPolicy.PolicyName.Should().BeEquivalentTo("Example");

            var context = new DslAuthorizationContext()
            {
                Subject = new Dictionary<string, object>()
                {
                    {"Age", 19 },
                    {"Id", 1 },
                }
            };

            dslAuthorizationPolicy.IsSatisfied(context).Should().BeTrue();
        }

        [Fact]
        public void visitor_allow_to_parse_primitive_aritmetic_comparer_expressions()
        {
            const string policy = @"
            policy Example begin
                rule CardiologyNurses (PERMIT) begin
                    Subject.Age < 10 * 2
                end
            end";

            var dslAuthorizationPolicy = DslAuthorizationPolicy.CreateFromGrammar(policy, Grammars.Bal);

            dslAuthorizationPolicy.PolicyName.Should().BeEquivalentTo("Example");

            var context = new DslAuthorizationContext()
            {
                Subject = new Dictionary<string, object>()
                {
                    {"Age", 19 },
                }
            };

            dslAuthorizationPolicy.IsSatisfied(context).Should().BeTrue();
        }

        [Fact]
        public void visitor_allow_to_parse_primitive_string_comparer_expressions()
        {
            const string policy = @"
            policy Example begin
                rule CardiologyNurses (PERMIT) begin
                    Subject.Name = ""Mary Joe""
                end
            end";

            var dslAuthorizationPolicy = DslAuthorizationPolicy.CreateFromGrammar(policy, Grammars.Bal);

            dslAuthorizationPolicy.PolicyName.Should().BeEquivalentTo("Example");

            var context = new DslAuthorizationContext()
            {
                Subject = new Dictionary<string, object>()
                {
                    {"Name", "Mary Joe" },
                }
            };

            dslAuthorizationPolicy.IsSatisfied(context).Should().BeTrue();
        }

        [Fact]
        public void visitor_throw_when_check_satisfied_if_context_does_not_contain_a_property()
        {
            const string policy = @"
            policy Example begin
                rule CardiologyNurses (PERMIT) begin
                    Subject.Role = ""Nurse""
                end
            end";

            var dslAuthorizationPolicy = DslAuthorizationPolicy.CreateFromGrammar(policy, Grammars.Bal);

            dslAuthorizationPolicy.PolicyName.Should().BeEquivalentTo("Example");

            var context = new DslAuthorizationContext()
            {
                Subject = new Dictionary<string, object>()
                {
                    {"Name", "Mary Joe" },
                }
            };

            Assert.Throws<InvalidOperationException>(() =>
            {
                dslAuthorizationPolicy.IsSatisfied(context);
            }).Message.Should().BeEquivalentTo("The  rule CardiologyNurses is evaluating a property that does not exist on actual DslAuthorizationContext");
        }
    }
}