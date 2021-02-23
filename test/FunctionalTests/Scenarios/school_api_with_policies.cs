using AutoFixture;
using FluentAssertions;
using FunctionalTests.Seedwork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FunctionalTests.Scenarios
{
    [Collection(nameof(TestServerCollectionFixture))]
    public class school_api_with_policies
    {
        private const string InvalidSub = "0";
        private readonly TestServerFixture fixture;
        private readonly IEnumerable<TestServer> servers;

        public school_api_with_policies(TestServerFixture fixture)
        {
            this.fixture = fixture;
            this.servers = fixture.Servers
                .Where(x => !x.SupportSchemes)
                .Select(x => x.TestServer);
        }

        [Fact]
        public async Task not_allow_to_view_grades_if_the_policie_is_not_satisfied()
        {
            foreach (var server in servers)
            {
                var response = await server
                    .CreateRequest(Api.School.GetAbacPolicy)
                    .WithIdentity(new Fixture().Sub(InvalidSub))
                    .GetAsync();

                response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
            }
        }

        [Fact]
        [ResetDatabase]
        public async Task allow_to_view_grades_if_the_policie_is_satisfied()
        {
            var application = await fixture.GivenAnApplication();
            var subject = await fixture.GivenAnSubject(Subs.Teacher);
            await fixture.GivenARole(Roles.Teacher, application, subject);

            await fixture.GivenAPolicy(application, "abac-policy", AbacPolicies.Substitute);

            foreach (var server in servers)
            {
                var response = await server
                    .CreateRequest(Api.School.GetAbacPolicy)
                    .WithIdentity(new Fixture().Sub(subject.Sub))
                    .GetAsync();

                response.StatusCode.Should().Be(StatusCodes.Status200OK);
            }
        }
    }
}
