using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using FunctionalTests.Seedwork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using System.Threading.Tasks;
using Xunit;

namespace FunctionalTests.Scenarios
{
    [Collection(nameof(TestServerCollectionFixture))]
    public class school_api
    {
        private readonly TestServerFixture fixture;
        private readonly IEnumerable<TestServer> servers;

        public school_api(TestServerFixture fixture)
        {
            this.fixture = fixture;
            this.servers = fixture.Servers
                .Where(x => !x.SupportSchemes)
                .Select(x => x.TestServer);
        }

        [Fact]
        public async Task not_allow_to_view_grades_if_the_user_is_not_authenticated()
        {
            foreach (var server in servers)
            {
                var response = await server
                    .CreateRequest(Api.School.GetGrades)
                    .GetAsync();

                response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
            }
        }

        [Fact]
        public async Task not_allow_to_view_grades_if_the_user_is_not_authorized()
        {
            foreach (var server in servers)
            {
                var response = await server
                    .CreateRequest(Api.School.GetGrades)
                    .WithIdentity(new Fixture().Custodian())
                    .GetAsync();

                response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
            }
        }

        [Fact]
        public async Task allow_to_view_grades_if_the_user_belongs_to_the_teacher_role()
        {
            foreach (var server in servers)
            {
                var response = await server
                    .CreateRequest(Api.School.GetGrades)
                    .WithIdentity(new Fixture().Teacher())
                    .GetAsync();

                response.StatusCode.Should().Be(StatusCodes.Status200OK);
            }
        }

        [Fact]
        public async Task not_allow_to_view_grades_if_the_user_not_belongs_to_the_teacher_role()
        {
            foreach (var server in servers)
            {
                var response = await server
                    .CreateRequest(Api.School.GetGrades)
                    .WithIdentity(new Fixture().Custodian())
                    .GetAsync();

                response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
            }
        }

        [Fact]
        [ResetDatabase]
        public async Task allow_to_edit_grades_if_the_user_belongs_to_the_teacher_role()
        {
            await fixture.GiveAnApplication();

            foreach (var server in servers)
            {
                var response = await server
                    .CreateRequest(Api.School.EditGrades)
                    .WithIdentity(new Fixture().Teacher())
                    .PutAsync();

                response.StatusCode.Should().Be(StatusCodes.Status200OK);
            }
        }

        [Fact]
        [ResetDatabase]
        public async Task allow_to_edit_grades_if_the_client_belongs_to_the_teacher_role()
        {
            await fixture.GiveAnApplication();

            foreach (var server in servers)
            {
                var response = await server
                    .CreateRequest(Api.School.EditGrades)
                    .WithIdentity(new Fixture().Client())
                    .PutAsync();

                response.StatusCode.Should().Be(StatusCodes.Status200OK);
            }
        }

        [Fact]
        [ResetDatabase]
        public async Task allow_to_edit_grades_if_someone_has_delegated_his_permissions()
        {
            await fixture.GiveAnApplication(selectedDelegation: true);

            foreach (var server in servers)
            {
                var response = await server
                    .CreateRequest(Api.School.EditGrades)
                    .WithIdentity(new Fixture().FirstSubstitute())
                    .PutAsync();

                response.StatusCode.Should().Be(StatusCodes.Status200OK);
            }
        }

        [Fact]
        [ResetDatabase]
        public async Task not_allow_to_edit_grades_if_someone_has_delegated_his_permissions_but_no_delegations_has_been_selected()
        {
            await fixture.GiveAnApplication(selectedDelegation: false);

            foreach (var server in servers)
            {
                var response = await server
                    .CreateRequest(Api.School.EditGrades)
                    .WithIdentity(new Fixture().SecondSubstitute())
                    .PutAsync();

                response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
            }
        }
        [Fact]
        [ResetDatabase]
        public async Task not_fails_when_endpoint_does_not_exists()
        {
            foreach (var server in fixture.Servers)
            {
                var response = await server
                    .CreateRequest(Api.School.InvalidEndpoint)
                    .WithIdentity(new Fixture().Client())
                    .GetAsync();

                response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            }
        }
    }
}
