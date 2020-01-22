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
            var response = await fixture
                .Server
                .CreateRequest("api/school/grades")
                .GetAsync();

            response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public async Task not_allow_to_view_grades_if_the_user_is_not_authorized()
        {
            var identity = Builders
                .Identity
                .Build();
            var response = await fixture
                .Server
                .CreateRequest("api/school/grades")
                .WithIdentity(identity)
                .GetAsync();

            response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
        }

        [Fact]
        public async Task to_view_grades_if_the_user_belongs_to_the_teacher_role()
        {
            var identity = Builders
                .Identity
                .Teacher();
            var response = await fixture
                .Server
                .CreateRequest("api/school/grades")
                .WithIdentity(identity)
                .GetAsync();

            response.StatusCode.Should().Be(StatusCodes.Status200OK);
        }
    }
}
