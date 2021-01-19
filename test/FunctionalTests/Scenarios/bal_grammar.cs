using Antlr4.Runtime;
using Balea.DSL;
using Balea.DSL.Grammar;
using Balea.DSL.Grammar.Bal;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace FunctionalTests.Scenarios
{
    public class bal_grammar
    {
        [Fact]
        public void visitor_allow_to_parse_logical_conditions()
        {
            const string policy = @"
            policy Example begin
                rule CardiologyNurses (PERMIT) begin
                    Subject.Role = ""Nurse"" AND  Resource.Action = ""MedicalRecord""
                end
            end";

            var dslAuthorizationPolicy = GetAuthorizationPolicy(policy);

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
        public void visitor_allow_to_parse_with_not_title_case()
        {
            const string policy = @"
            policy Example begin
                rule CardiologyNurses (PERMIT) begin
                    suBjEct.roLe = ""Nurse"" AND  reSourcE.acTion = ""MedicalRecord""
                end
            end";

            var dslAuthorizationPolicy = GetAuthorizationPolicy(policy);

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

            var dslAuthorizationPolicy = GetAuthorizationPolicy(policy);

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

            var dslAuthorizationPolicy = GetAuthorizationPolicy(policy);

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

            var dslAuthorizationPolicy = GetAuthorizationPolicy(policy);

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

            var dslAuthorizationPolicy = GetAuthorizationPolicy(policy);

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

            var dslAuthorizationPolicy = GetAuthorizationPolicy(policy);

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

        //[Fact]
        //public void visitor_allow_to_parse_aritmetic_operations_with_context_data()
        //{
        //    const string policy = @"
        //    policy Example begin
        //        rule CardiologyNurses (PERMIT) begin
        //            Subject.Age < 20 AND  Subject.Id * 1000 >= 1000
        //        end
        //    end";

        //    var dslAuthorizationPolicy = GetAuthorizationPolicy(policy);

        //    dslAuthorizationPolicy.PolicyName.Should().BeEquivalentTo("Example");

        //    var context = new DslAuthorizationContext()
        //    {
        //        Subject = new Dictionary<string, object>()
        //        {
        //            {"Age", 19 },
        //            {"Id", 1 },
        //        }
        //    };

        //    dslAuthorizationPolicy.IsSatisfied(context).Should().BeTrue();
        //}

        private DslAuthorizationPolicy GetAuthorizationPolicy(string policy)
        {
            var inputStream = new AntlrInputStream(policy);
            var lexer = new BalLexer(inputStream);
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new BalParser(tokenStream);

            return new BalVisitor().Visit(parser.policy());
        }
    }
}
