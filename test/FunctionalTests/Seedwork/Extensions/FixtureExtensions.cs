using AutoFixture;
using System;
using System.Threading.Tasks;
using Volvoreta;
using Volvoreta.EntityFrameworkCore.Store.Entities;

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

                db.Add(john);
                db.Add(mary);
                db.Add(anna);

                await db.SaveChangesAsync();

                var application = new ApplicationEntity(VolvoretaConstants.DefaultApplicationName, "Default application");
                var viewGradesPermission = new PermissionEntity(Policies.ViewGrades);
                var editGradesPermission = new PermissionEntity(Policies.EditGrades);
                application.Permissions.Add(viewGradesPermission);
                application.Permissions.Add(editGradesPermission);
                var teacherRole = new RoleEntity("Teacher", "Teacher role");
                teacherRole.Subjects.Add(new RoleSubjectEntity { SubjectId = john.Id });
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
