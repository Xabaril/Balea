using Balea.DSL;
using Balea.DSL.Grammar;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace FunctionalTests.Scenarios
{
#pragma warning disable IDE1006
#pragma warning disable IDE0044
    public class bal_grammar

    {
        [Fact]
        public async Task visitor_allow_to_parse_and_logical_conditions()
        {
            const string policy = @"
            policy Example begin
                rule CardiologyNurses (PERMIT) begin
                    Subject.Role = ""Nurse"" AND  Resource.Action = ""MedicalRecord""
                end
            end";

            var dslAuthorizationPolicy = AbacAuthorizationPolicy.CreateFromGrammar(policy, AllowedGrammars.Bal);

            dslAuthorizationPolicy.PolicyName.Should().BeEquivalentTo("Example");

            var propertyBags = new List<IPropertyBag>()
            {
                new TestPropertyBag("Subject", new Dictionary<string, object>()
                {
                       {"Role", "Nurse" }
                }),
                 new TestPropertyBag("Resource", new Dictionary<string, object>()
                {
                       {"Action", "MedicalRecord" }
                })
            };

            var contextFactory = new AbacAuthorizationContextFactory(propertyBags);
            var context = await contextFactory.Create(null);

            dslAuthorizationPolicy.IsSatisfied(context).Should().BeTrue();
        }

        [Fact]
        public async Task visitor_allow_to_parse_or_logical_conditions()
        {
            const string policy = @"
            policy Example begin
                rule CardiologyNurses (PERMIT) begin
                    Subject.Name = ""Mary Joe"" OR  Subject.Name = ""Jhon Doe""
                end
            end";

            var dslAuthorizationPolicy = AbacAuthorizationPolicy.CreateFromGrammar(policy, AllowedGrammars.Bal);

            dslAuthorizationPolicy.PolicyName.Should().BeEquivalentTo("Example");

            var propertyBags = new List<IPropertyBag>()
            {
                new TestPropertyBag("Subject", new Dictionary<string, object>()
                {
                      {"Name", "Scott Hunter" }
                })
            };

            var contextFactory = new AbacAuthorizationContextFactory(propertyBags);
            var context = await contextFactory.Create(null);

            dslAuthorizationPolicy.IsSatisfied(context).Should().BeFalse();

            propertyBags = new List<IPropertyBag>()
            {
                new TestPropertyBag("Subject", new Dictionary<string, object>()
                {
                      {"Name", "Mary Joe" }
                })
            };

            contextFactory = new AbacAuthorizationContextFactory(propertyBags);
            context = await contextFactory.Create(null);

            dslAuthorizationPolicy.IsSatisfied(context).Should().BeTrue();
        }

        [Fact]
        public async Task visitor_allow_to_parse_with_not_title_case()
        {
            const string policy = @"
            policy Example begin
                rule CardiologyNurses (PERMIT) begin
                    suBjEct.roLe = ""Nurse"" AND  reSourcE.acTion = ""MedicalRecord""
                end
            end";

            var dslAuthorizationPolicy = AbacAuthorizationPolicy.CreateFromGrammar(policy, AllowedGrammars.Bal);

            dslAuthorizationPolicy.PolicyName.Should().BeEquivalentTo("Example");

            var propertyBags = new List<IPropertyBag>()
            {
                new TestPropertyBag("Subject", new Dictionary<string, object>()
                {
                     {"Role", "Nurse" },
                }),
                new TestPropertyBag("Resource", new Dictionary<string, object>()
                {
                    {"Action", "MedicalRecord" }
                })
            };

            var contextFactory = new AbacAuthorizationContextFactory(propertyBags);
            var context = await contextFactory.Create(null);

            dslAuthorizationPolicy.IsSatisfied(context).Should().BeTrue();
        }

        [Fact]
        public async Task visitor_allow_to_parse_multiple_logical_conditions()
        {
            const string policy = @"
            policy Example begin
                rule CardiologyNurses (PERMIT) begin
                    Subject.Role = ""Nurse"" 
                    AND Subject.Name = ""Mary Joe""
                    AND Resource.Action = ""MedicalRecord""
                end
            end";

            var dslAuthorizationPolicy = AbacAuthorizationPolicy.CreateFromGrammar(policy, AllowedGrammars.Bal);

            dslAuthorizationPolicy.PolicyName.Should().BeEquivalentTo("Example");

            var propertyBags = new List<IPropertyBag>()
            {
                new TestPropertyBag("Subject", new Dictionary<string, object>()
                {
                    {"Role", "Nurse" },
                     {"Name", "Jhon Doe" },
                }),
                new TestPropertyBag("Resource", new Dictionary<string, object>()
                {
                    {"Action", "MedicalRecord" }
                })
            };

            var contextFactory = new AbacAuthorizationContextFactory(propertyBags);
            var context = await contextFactory.Create(null);

            dslAuthorizationPolicy.IsSatisfied(context).Should().BeFalse();
        }

        [Fact]
        public async Task visitor_allow_to_use_rule_action()
        {
            const string policy = @"
            policy Example begin
                rule CardiologyNurses (DENY) begin
                    Subject.Role = ""Nurse"" 
                    AND Subject.Name = ""Mary Joe""
                end
            end";

            var dslAuthorizationPolicy = AbacAuthorizationPolicy.CreateFromGrammar(policy, AllowedGrammars.Bal);

            dslAuthorizationPolicy.PolicyName.Should().BeEquivalentTo("Example");

            var propertyBags = new List<IPropertyBag>()
            {
                new TestPropertyBag("Subject", new Dictionary<string, object>()
                {
                     {"Role", "Nurse" },
                     {"Name", "Mary Joe" },
                }),
                 new TestPropertyBag("Resource", new Dictionary<string, object>()
                {
                     {"Action", "MedicalRecord" }
                })
            };

            var contextFactory = new AbacAuthorizationContextFactory(propertyBags);
            var context = await contextFactory.Create(null);

            dslAuthorizationPolicy.IsSatisfied(context).Should().BeFalse();
        }

        [Fact]
        public async Task visitor_allow_to_use_multiple_rules()
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

            var dslAuthorizationPolicy = AbacAuthorizationPolicy.CreateFromGrammar(policy, AllowedGrammars.Bal);

            dslAuthorizationPolicy.PolicyName.Should().BeEquivalentTo("Example");

            var propertyBags = new List<IPropertyBag>()
            {
                new TestPropertyBag("Subject", new Dictionary<string, object>()
                {
                    {"Role", "Nurse" },
                    {"Name", "Mary Joe" },
                }),
                new TestPropertyBag("Resource", new Dictionary<string, object>()
                {
                    {"Action", "medicalreports" }
                })
            };

            var contextFactory = new AbacAuthorizationContextFactory(propertyBags);
            var context = await contextFactory.Create(null);

            dslAuthorizationPolicy.IsSatisfied(context)
                .Should().BeTrue();

            propertyBags = new List<IPropertyBag>()
            {
                new TestPropertyBag("Subject", new Dictionary<string, object>()
                {
                    {"Role", "Nurse" },
                    {"Name", "Jhon Doe" },
                }),
                new TestPropertyBag("Resource", new Dictionary<string, object>()
                {
                    {"Action", "medicalreports" }
                })
            };

            contextFactory = new AbacAuthorizationContextFactory(propertyBags);
            context = await contextFactory.Create(null);

            dslAuthorizationPolicy.IsSatisfied(context).Should().BeFalse();
        }

        [Fact]
        public async Task visitor_allow_to_parse_aritmetic_conditions()
        {
            const string policy = @"
            policy Example begin
                rule CardiologyNurses (PERMIT) begin
                    Subject.Age < 20 AND  Resource.Id > 1000
                end
            end";

            var dslAuthorizationPolicy = AbacAuthorizationPolicy.CreateFromGrammar(policy, AllowedGrammars.Bal);

            dslAuthorizationPolicy.PolicyName.Should().BeEquivalentTo("Example");

            var propertyBags = new List<IPropertyBag>()
            {
                new TestPropertyBag("Subject", new Dictionary<string, object>()
                {
                    {"Age", 19 },
                }),
                new TestPropertyBag("Resource", new Dictionary<string, object>()
                {
                    {"Id", 1001 },
                })
            };

            var contextFactory = new AbacAuthorizationContextFactory(propertyBags);
            var context = await contextFactory.Create(null);

            dslAuthorizationPolicy.IsSatisfied(context).Should().BeTrue();
        }

        [Fact]
        public async Task visitor_allow_to_parse_aritmetic_operations()
        {
            const string policy = @"
            policy Example begin
                rule CardiologyNurses (PERMIT) begin
                    Subject.Role = ""Nurse"" AND  Resource.Id > (10 * 100 * 10)
                end
            end";

            var dslAuthorizationPolicy = AbacAuthorizationPolicy.CreateFromGrammar(policy, AllowedGrammars.Bal);

            dslAuthorizationPolicy.PolicyName.Should().BeEquivalentTo("Example");

            var propertyBags = new List<IPropertyBag>()
            {
                new TestPropertyBag("Subject", new Dictionary<string, object>()
                {
                    {"Role", "Nurse" },
                }),
                new TestPropertyBag("Resource", new Dictionary<string, object>()
                {
                    {"Id", 999 },
                })
            };

            var contextFactory = new AbacAuthorizationContextFactory(propertyBags);
            var context = await contextFactory.Create(null);

            dslAuthorizationPolicy.IsSatisfied(context).Should().BeFalse();
        }

        [Fact]
        public async Task visitor_allow_to_parse_aritmetic_operations_with_context_data()
        {
            const string policy = @"
            policy Example begin
                rule CardiologyNurses (PERMIT) begin
                    Subject.Age < 20 AND  Subject.Id * 1000 >= 1000 * 1
                end
            end";

            var dslAuthorizationPolicy = AbacAuthorizationPolicy.CreateFromGrammar(policy, AllowedGrammars.Bal);

            dslAuthorizationPolicy.PolicyName.Should().BeEquivalentTo("Example");

            var propertyBags = new List<IPropertyBag>()
            {
                new TestPropertyBag("Subject", new Dictionary<string, object>()
                {
                    {"Age", 19 },
                    {"Id",1 }
                })
            };

            var contextFactory = new AbacAuthorizationContextFactory(propertyBags);
            var context = await contextFactory.Create(null);

            dslAuthorizationPolicy.IsSatisfied(context).Should().BeTrue();
        }

        [Fact]
        public async Task visitor_allow_to_parse_primitive_aritmetic_comparer_expressions()
        {
            const string policy = @"
            policy Example begin
                rule CardiologyNurses (PERMIT) begin
                    Subject.Age < 10 * 2
                end
            end";

            var dslAuthorizationPolicy = AbacAuthorizationPolicy.CreateFromGrammar(policy, AllowedGrammars.Bal);

            dslAuthorizationPolicy.PolicyName.Should().BeEquivalentTo("Example");

            var propertyBags = new List<IPropertyBag>()
            {
                new TestPropertyBag("Subject", new Dictionary<string, object>()
                {
                    {"Age", 19 },
                })
            };

            var contextFactory = new AbacAuthorizationContextFactory(propertyBags);
            var context = await contextFactory.Create(null);

            dslAuthorizationPolicy.IsSatisfied(context).Should().BeTrue();
        }

        [Fact]
        public async Task visitor_allow_to_parse_primitive_string_comparer_expressions()
        {
            const string policy = @"
            policy Example begin
                rule CardiologyNurses (PERMIT) begin
                    Subject.Name = ""Mary Joe""
                end
            end";

            var dslAuthorizationPolicy = AbacAuthorizationPolicy.CreateFromGrammar(policy, AllowedGrammars.Bal);

            dslAuthorizationPolicy.PolicyName.Should().BeEquivalentTo("Example");

            var propertyBags = new List<IPropertyBag>()
            {
                new TestPropertyBag("Subject", new Dictionary<string, object>()
                {
                    {"Name", "Mary Joe" },
                })
            };

            var contextFactory = new AbacAuthorizationContextFactory(propertyBags);
            var context = await contextFactory.Create(null);

            dslAuthorizationPolicy.IsSatisfied(context).Should().BeTrue();
        }

        [Fact]
        public async Task visitor_throw_when_check_satisfied_if_context_does_not_contain_a_property()
        {
            const string policy = @"
            policy Example begin
                rule CardiologyNurses (PERMIT) begin
                    Subject.Role = ""Nurse""
                end
            end";

            var dslAuthorizationPolicy = AbacAuthorizationPolicy.CreateFromGrammar(policy, AllowedGrammars.Bal);

            dslAuthorizationPolicy.PolicyName.Should().BeEquivalentTo("Example");

            var propertyBags = new List<IPropertyBag>()
            {
                new TestPropertyBag("Subject", new Dictionary<string, object>()
                {
                    {"Name", "Mary Joe" },
                })
            };

            var contextFactory = new AbacAuthorizationContextFactory(propertyBags);
            var context = await contextFactory.Create(null);

            Assert.Throws<InvalidOperationException>(() =>
            {
                dslAuthorizationPolicy.IsSatisfied(context);
            }).Message.Should().BeEquivalentTo("The rule CardiologyNurses is evaluating a property that does not exist on actual DslAuthorizationContext");
        }

        [Fact]
        public async Task visitor_allow_to_parse_complex_expressions()
        {
            const string policy = @"
            policy Example begin
                rule CardiologyNurses (PERMIT) begin
                     ( Subject.Role = ""Nurse"" OR Subject.Role = ""Doctor"" ) AND Resource.Action = ""medicalreports""
                end
            end";

            var dslAuthorizationPolicy = AbacAuthorizationPolicy.CreateFromGrammar(policy, AllowedGrammars.Bal);

            dslAuthorizationPolicy.PolicyName.Should().BeEquivalentTo("Example");

            var propertyBags = new List<IPropertyBag>()
            {
                new TestPropertyBag("Subject", new Dictionary<string, object>()
                {
                    {"Role", "Nurse" },
                }),
                new TestPropertyBag("Resource", new Dictionary<string, object>()
                {
                    {"Action", "medicalreports" }
                })
            };

            var contextFactory = new AbacAuthorizationContextFactory(propertyBags);
            var context = await contextFactory.Create(null);

            dslAuthorizationPolicy.IsSatisfied(context)
                .Should()
                .BeTrue();

            propertyBags = new List<IPropertyBag>()
            {
                new TestPropertyBag("Subject", new Dictionary<string, object>()
                {
                    {"Role", "Doctor" },
                }),
                new TestPropertyBag("Resource", new Dictionary<string, object>()
                {
                    {"Action", "medicalreports" }
                })
            };

            contextFactory = new AbacAuthorizationContextFactory(propertyBags);
            context = await contextFactory.Create(null);

            dslAuthorizationPolicy.IsSatisfied(context)
                .Should()
                .BeTrue();

            propertyBags = new List<IPropertyBag>()
            {
                new TestPropertyBag("Subject", new Dictionary<string, object>()
                {
                    {"Role", "Doctor" },
                }),
                new TestPropertyBag("Resource", new Dictionary<string, object>()
                {
                    {"Action", "schedulingreports" }
                })
            };

            contextFactory = new AbacAuthorizationContextFactory(propertyBags);
            context = await contextFactory.Create(null);

            dslAuthorizationPolicy.IsSatisfied(context)
                .Should()
                .BeFalse();
        }

        private class TestPropertyBag
            : IPropertyBag
        {
            private string _name;
            private Dictionary<string, object> _items;

            public TestPropertyBag(string name, Dictionary<string, object> items)
            {
                _name = name;
                _items = items;
            }

            public string Name => _name;

            public Task Initialize(AuthorizationHandlerContext state)
            {
                return Task.CompletedTask;
            }

            public object this[string name]
            {
                get
                {
                    return _items[name];
                }
            }
        }
    }
#pragma warning restore IDE1006
#pragma warning restore IDE0044
}
