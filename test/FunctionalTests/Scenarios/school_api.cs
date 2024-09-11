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
        private const string InvalidSub = "0";
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

                response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
            }
        }

        [Fact]
        public async Task not_allow_to_view_grades_if_the_user_is_not_authorized()
        {
            foreach (var server in servers)
            {
                var response = await server
                    .CreateRequest(Api.School.GetGrades)
                    .WithIdentity(new Fixture().Sub(InvalidSub))
                    .GetAsync();

                response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
            }
        }

        [Fact]
        [ResetDatabase]
        public async Task allow_to_view_grades_if_the_user_have_permission()
        {
            var application = await fixture.GivenAnApplication();
            var subject = await fixture.GivenAnSubject(Subs.Teacher);
            await fixture.GivenARole(Roles.Teacher, application, subject);

            foreach (var server in servers)
            {
                var response = await server
                    .CreateRequest(Api.School.GetGrades)
                    .WithIdentity(new Fixture().Sub(subject.Sub))
                    .GetAsync();

                response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            }
        }

        [Fact]
        [ResetDatabase]
        public async Task not_allow_to_view_grades_if_the_user_does_not_have_permissions()
        {
            await fixture.GivenAnApplication();
            var subject = await fixture.GivenAnSubject(Subs.SubstituteTwo);

            foreach (var server in servers)
            {
                var response = await server
                    .CreateRequest(Api.School.GetGrades)
                    .WithIdentity(new Fixture().Sub(subject.Sub))
                    .GetAsync();

                response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
            }
        }

        [Fact]
        [ResetDatabase]
        public async Task allow_to_edit_grades_if_the_user_does_have_permission()
        {
            var application = await fixture.GivenAnApplication();
            var subject = await fixture.GivenAnSubject(Subs.Teacher);
            await fixture.GivenARole(Roles.Teacher, application, subject);

            foreach (var server in servers)
            {
                var response = await server
                    .CreateRequest(Api.School.EditGrades)
                    .WithIdentity(new Fixture().Sub(subject.Sub))
                    .PutAsync();

                response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            }
        }

        [Fact]
        [ResetDatabase]
        public async Task allow_to_edit_grades_if_the_client_does_have_permission()
        {
            var application = await fixture.GivenAnApplication();
            var subject = await fixture.GivenAnSubject(Subs.Teacher);
            await fixture.GivenARole(Roles.Teacher, application, subject);

            foreach (var server in servers)
            {
                var response = await server
                    .CreateRequest(Api.School.EditGrades)
                    .WithIdentity(new Fixture().Client(subject.Sub))
                    .PutAsync();

                response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            }
        }

        [Fact]
        [ResetDatabase]
        public async Task allow_to_edit_grades_if_the_upn_user_does_have_permission()
        {
            var application = await fixture.GivenAnApplication();
            var subject = await fixture.GivenAnSubject(Subs.Teacher);
            await fixture.GivenARole(Roles.Teacher, application, subject);

            foreach (var server in servers)
            {
                var response = await server
                    .CreateRequest(Api.School.EditGrades)
                    .WithIdentity(new Fixture().UpnSub(subject.Sub))
                    .PutAsync();

                response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            }
        }


        [Fact]
        [ResetDatabase]
        public async Task allow_to_edit_grades_if_someone_has_delegated_his_permissions()
        {
            var application = await fixture.GivenAnApplication();
            var who = await fixture.GivenAnSubject(Subs.Teacher);
            var whom = await fixture.GivenAnSubject(Subs.SubstituteOne);
            await fixture.GivenARole(Roles.Teacher, application, who);
            await fixture.GivenAnUserWithADelegation(application, who, whom);

            foreach (var server in servers)
            {
                var response = await server
                    .CreateRequest(Api.School.EditGrades)
                    .WithIdentity(new Fixture().Sub(whom.Sub))
                    .PutAsync();

                response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            }
        }

        [Fact]
        [ResetDatabase]
        public async Task not_allow_to_edit_grades_if_someone_has_delegated_his_permissions_but_no_delegations_has_been_selected()
        {
            var application = await fixture.GivenAnApplication();
            var who = await fixture.GivenAnSubject(Subs.Teacher);
            var whom = await fixture.GivenAnSubject(Subs.SubstituteTwo);
            await fixture.GivenARole(Roles.Teacher, application, who);
            await fixture.GivenAnUserWithADelegation(application, who, whom, selected: false);

            foreach (var server in servers)
            {
                var response = await server
                    .CreateRequest(Api.School.EditGrades)
                    .WithIdentity(new Fixture().Sub(whom.Sub))
                    .PutAsync();

                response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
            }
        }

        [Fact]
        [ResetDatabase]
        public async Task not_allow_to_edit_grades_if_someone_has_delegated_his_permissions_in_another_application()
        {
            var application = await fixture.GivenAnApplication();
            var anotherApplication = await fixture.GivenAnApplication(applicationName: "another");
            var who = await fixture.GivenAnSubject(Subs.SubstituteOne);
            var whom = await fixture.GivenAnSubject(Subs.SubstituteTwo);
            await fixture.GivenARole(Roles.Teacher, application, who);
            await fixture.GivenAnUserWithADelegation(anotherApplication, who, whom, selected: true);

            foreach (var server in servers)
            {
                var response = await server
                    .CreateRequest(Api.School.EditGrades)
                    .WithIdentity(new Fixture().Sub(whom.Sub))
                    .PutAsync();

                response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
            }
        }
    }
}
