using FluentAssertions;
using FunctionalTests.Seedwork;
using FunctionalTests.Seedwork.Builders;
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

        public school_api(TestServerFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task not_allow_to_view_grades_if_the_user_is_not_authenticated()
        {
            foreach (var server in fixture.Servers)
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
            var identity = Builders
                .Identity
                .Build();

            foreach (var server in fixture.Servers)
            {
                var response = await server
                    .CreateRequest(Api.School.GetGrades)
                    .WithIdentity(identity)
                    .GetAsync();

                response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
            }
        }

        [Fact]
        public async Task to_view_grades_if_the_user_belongs_to_the_teacher_role()
        {
            var identity = Builders
                .Identity
                .Teacher();

            foreach (var server in fixture.Servers)
            {
                var response = await server
                    .CreateRequest(Api.School.GetGrades)
                    .WithIdentity(identity)
                    .GetAsync();

                response.StatusCode.Should().Be(StatusCodes.Status200OK);
            }
        }

        [Fact]
        public async Task not_allow_to_view_grades_if_the_user_not_belongs_to_the_teacher_role()
        {
            var identity = Builders
                .Identity
                .Custodian();

            foreach (var server in fixture.Servers)
            {
                var response = await server
                    .CreateRequest(Api.School.GetGrades)
                    .WithIdentity(identity)
                    .GetAsync();

                response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
            }
        }

        [Fact]
        [ResetDatabase]
        public async Task to_edit_grades_if_the_user_belongs_to_the_teacher_role()
        {
            var identity = Builders
                .Identity
                .Teacher();

            await fixture.GiveAnApplicationWithTeacherRole();

            foreach (var server in fixture.Servers)
            {
                var response = await server
                    .CreateRequest(Api.School.EditGrades)
                    .WithIdentity(identity)
                    .PutAsync();

                response.StatusCode.Should().Be(StatusCodes.Status200OK);
            }
        }
    }
}
