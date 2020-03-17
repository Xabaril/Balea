using AutoFixture;
using System;
using System.Threading.Tasks;
using Balea;
using Balea.EntityFrameworkCore.Store.Entities;

namespace FunctionalTests.Seedwork
{
    public static class FixtureExtensions
    {
        public static async Task GiveAnApplication(this TestServerFixture fixture, bool selectedDelegation = false)
        {
            await fixture.ExecuteDbContextAsync(async db =>
            {
                var john = new SubjectEntity("John", AutoFixtureExtensions.TeacherSub);
                var mary = new SubjectEntity("Mary", AutoFixtureExtensions.FirstSubstituteSub);
                var anna = new SubjectEntity("Anna", AutoFixtureExtensions.SecondSubstituteSub);
                var client = new SubjectEntity("Client", AutoFixtureExtensions.ClientId);

                db.Add(john);
                db.Add(mary);
                db.Add(anna);
                db.Add(client);

                await db.SaveChangesAsync();

                var application = new ApplicationEntity(BaleaConstants.DefaultApplicationName, "Default application");
                var viewGradesPermission = new PermissionEntity(Policies.ViewGrades);
                var editGradesPermission = new PermissionEntity(Policies.EditGrades);
                application.Permissions.Add(viewGradesPermission);
                application.Permissions.Add(editGradesPermission);
                var teacherRole = new RoleEntity("Teacher", "Teacher role");
                teacherRole.Subjects.Add(new RoleSubjectEntity { SubjectId = john.Id });
                teacherRole.Subjects.Add(new RoleSubjectEntity { SubjectId = client.Id});
                teacherRole.Permissions.Add(new RolePermissionEntity { Permission = viewGradesPermission });
                teacherRole.Permissions.Add(new RolePermissionEntity { Permission = editGradesPermission });
                application.Roles.Add(teacherRole);
                application.Delegations.Add(new DelegationEntity(john.Id, mary.Id, DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(1), selectedDelegation));
                application.Delegations.Add(new DelegationEntity(john.Id, anna.Id, DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(1), selectedDelegation));
                db.Applications.Add(application);
                await db.SaveChangesAsync();
            });
        }
    }
}
