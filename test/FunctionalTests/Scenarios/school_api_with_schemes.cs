using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using AutoFixture;
using FluentAssertions;
using FunctionalTests.Seedwork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

namespace FunctionalTests.Scenarios
{
    [Collection(nameof(TestServerCollectionFixture))]
    public class school_api_with_schemes
    {
        private const string DefaultScheme = "scheme1";
        private const string BaleaScheme = "scheme2";
        private const string NotBaleaScheme = "scheme3";

        private readonly TestServerFixture fixture;
        private readonly IEnumerable<TestServer> servers;

        public school_api_with_schemes(TestServerFixture fixture)
        {
            this.fixture = fixture;
            this.servers = fixture.Servers
                .Where(x => x.SupportSchemes)
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
        public async Task not_allow_to_view_grades_if_the_user_is_authenticated_with_non_balea_schema_and_not_authorized()
        {
            foreach (var server in servers)
            {
                var response = await server
                    .CreateRequest(Api.School.GetGrades)
                    .WithIdentity(new Fixture().Custodian(), DefaultScheme)
                    .GetAsync();

                response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
            }
        }

        [Fact]
        public async Task not_allow_to_view_grades_if_the_user_is_authenticated_with_balea_schema_and_not_authorized()
        {
            foreach (var server in servers)
            {
                var response = await server
                    .CreateRequest(Api.School.GetGrades)
                    .WithIdentity(new Fixture().Custodian(), BaleaScheme)
                    .GetAsync();

                response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
            }
        }

        [Fact]
        public async Task not_allow_to_view_grades_if_the_user_is_authenticated_with_non_balea_schema_and_belongs_to_the_teacher_role()
        {
            foreach (var server in servers)
            {
                var response = await server
                    .CreateRequest(Api.School.GetGrades)
                    .WithIdentity(new Fixture().Teacher(), DefaultScheme)
                    .GetAsync();

                response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
            }
        }

        [Fact]
        public async Task allow_to_view_grades_if_the_user_is_authenticated_with_balea_schema_and_belongs_to_the_teacher_role()
        {
            foreach (var server in servers)
            {
                var response = await server
                    .CreateRequest(Api.School.GetGrades)
                    .WithIdentity(new Fixture().Teacher(), BaleaScheme)
                    .GetAsync();

                await response.IsSuccessStatusCodeOrThrow();

                var schemes = JsonConvert.DeserializeObject<string[]>(await response.Content.ReadAsStringAsync());

                schemes.Should().HaveCount(2);
                schemes.Should().Contain(BaleaScheme);
                schemes.Should().Contain("Balea");
            }
        }

        [Fact]
        public async Task not_allow_to_view_grades_if_the_user_is_authenticated_with_non_balea_schema_and_not_belongs_to_the_teacher_role()
        {
            foreach (var server in servers)
            {
                var response = await server
                    .CreateRequest(Api.School.GetGrades)
                    .WithIdentity(new Fixture().Custodian(), DefaultScheme)
                    .GetAsync();

                response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
            }
        }

        [Fact]
        public async Task not_allow_to_view_grades_if_the_user_is_authenticated_with_balea_schema_and_not_belongs_to_the_teacher_role()
        {
            foreach (var server in servers)
            {
                var response = await server
                    .CreateRequest(Api.School.GetGrades)
                    .WithIdentity(new Fixture().Custodian(), BaleaScheme)
                    .GetAsync();

                response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
            }
        }

        [Fact]
        public async Task call_to_endpoint_authorized_with_default_non_balea_scheme_should_not_include_balea()
        {
            foreach (var server in servers)
            {
                var response = await server
                    .CreateRequest(Api.School.GetSchemes)
                    .WithIdentity(new Fixture().Custodian(), DefaultScheme)
                    .GetAsync();

                await response.IsSuccessStatusCodeOrThrow();

                var schemes = JsonConvert.DeserializeObject<string[]>(await response.Content.ReadAsStringAsync());

                schemes.Should().HaveCount(1);
                schemes.Should().Contain(DefaultScheme);
                schemes.Should().NotContain("Balea");
            }
        }

        [Fact]
        public async Task call_to_endpoint_authorized_with_not_balea_configured_scheme_should_not_include_balea()
        {
            foreach (var server in servers)
            {
                var response = await server
                    .CreateRequest(Api.School.GetCustomPolicy)
                    .WithIdentity(new Fixture().Custodian(), NotBaleaScheme)
                    .GetAsync();

                await response.IsSuccessStatusCodeOrThrow();

                var schemes = JsonConvert.DeserializeObject<string[]>(await response.Content.ReadAsStringAsync());

                schemes.Should().HaveCount(1);
                schemes.Should().Contain(NotBaleaScheme);
                schemes.Should().NotContain("Balea");
            }
        }

        //[Fact]
        //[ResetDatabase]
        //public async Task allow_to_edit_grades_if_the_user_belongs_to_the_teacher_role()
        //{
        //    await fixture.GiveAnApplication();

        //    foreach (var server in servers)
        //    {
        //        var response = await server
        //            .CreateRequest(Api.School.EditGrades)
        //            .WithIdentity(new Fixture().Teacher())
        //            .PutAsync();

        //        response.StatusCode.Should().Be(StatusCodes.Status200OK);
        //    }
        //}

        //[Fact]
        //[ResetDatabase]
        //public async Task allow_to_edit_grades_if_the_client_belongs_to_the_teacher_role()
        //{
        //    await fixture.GiveAnApplication();

        //    foreach (var server in servers)
        //    {
        //        var response = await server
        //            .CreateRequest(Api.School.EditGrades)
        //            .WithIdentity(new Fixture().Client())
        //            .PutAsync();

        //        response.StatusCode.Should().Be(StatusCodes.Status200OK);
        //    }
        //}

        //[Fact]
        //[ResetDatabase]
        //public async Task allow_to_edit_grades_if_someone_has_delegated_his_permissions()
        //{
        //    await fixture.GiveAnApplication(selectedDelegation: true);

        //    foreach (var server in servers)
        //    {
        //        var response = await server
        //            .CreateRequest(Api.School.EditGrades)
        //            .WithIdentity(new Fixture().FirstSubstitute())
        //            .PutAsync();

        //        response.StatusCode.Should().Be(StatusCodes.Status200OK);
        //    }
        //}

        //[Fact]
        //[ResetDatabase]
        //public async Task not_allow_to_edit_grades_if_someone_has_delegated_his_permissions_but_no_delegations_has_been_selected()
        //{
        //    await fixture.GiveAnApplication(selectedDelegation: false);

        //    foreach (var server in servers)
        //    {
        //        var response = await server
        //            .CreateRequest(Api.School.EditGrades)
        //            .WithIdentity(new Fixture().SecondSubstitute())
        //            .PutAsync();

        //        response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
        //    }
        //}
    }
}
