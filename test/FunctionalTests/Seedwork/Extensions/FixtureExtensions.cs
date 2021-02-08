using Balea.EntityFrameworkCore.Store.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace FunctionalTests.Seedwork
{
    public static class FixtureExtensions
    {
        public static async Task<ApplicationEntity> GivenAnApplication(
            this TestServerFixture fixture,
            string applicationName = global::Balea.BaleaConstants.DefaultApplicationName)
        {
            var application = new ApplicationEntity(applicationName, applicationName);
            await fixture.ExecuteDbContextAsync(async db =>
            {
                var viewGradesPermission = new PermissionEntity(Policies.ViewGrades);
                var editGradesPermission = new PermissionEntity(Policies.EditGrades);
                application.Permissions.Add(viewGradesPermission);
                application.Permissions.Add(editGradesPermission);
                db.Applications.Add(application);
                await db.SaveChangesAsync();
            });
            return application;
        }

        public static async Task<SubjectEntity> GivenAnSubject(this TestServerFixture fixture, string sub)
        {
            var subject = new SubjectEntity(sub, sub);

            await fixture.ExecuteDbContextAsync(async db =>
            {
                db.Add(subject);

                await db.SaveChangesAsync();
            });

            return subject;
        }

        public static async Task<RoleEntity> GivenARole(
            this TestServerFixture fixture,
            string name,
            ApplicationEntity application,
            SubjectEntity subject,
            bool withPermissions = true)
        {
            var role = new RoleEntity(name, application.Id, $"{name} role");

            await fixture.ExecuteDbContextAsync(async db =>
            {
                role.Subjects.Add(new RoleSubjectEntity { SubjectId = subject.Id });
                
                if (withPermissions)
                {
                    foreach (var permission in application.Permissions)
                    {
                        role.Permissions.Add(new RolePermissionEntity { PermissionId = permission.Id });
                    }
                }

                db.Add(role);

                await db.SaveChangesAsync();
            });

            return role;
        }

        public static async Task GivenAnUserWithADelegation(
            this TestServerFixture fixture,
            ApplicationEntity application,
            SubjectEntity who,
            SubjectEntity whom,
            bool selected = true)
        {
            await fixture.ExecuteDbContextAsync(async db =>
            {
                var app = await db.Applications.SingleAsync(a => a.Id == application.Id);

                app.Delegations.Add(new DelegationEntity(who.Id, whom.Id, DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(1), selected));

                await db.SaveChangesAsync();
            });
        }
    }
}
