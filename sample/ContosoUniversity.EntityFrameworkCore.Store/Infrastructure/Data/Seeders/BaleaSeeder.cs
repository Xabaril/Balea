using System;
using System.Linq;
using System.Threading.Tasks;
using Balea;
using Balea.EntityFrameworkCore.Store.DbContexts;
using Balea.EntityFrameworkCore.Store.Entities;
using ContosoUniversity.EntityFrameworkCore.Store.Models;

namespace ContosoUniversity.EntityFrameworkCore.Store.Infrastructure.Data.Seeders
{
    public static class BaleaSeeder
    {
        public static async Task Seed(BaleaDbContext db)
        {
            if (!db.Roles.Any())
            {
                var alice = new SubjectEntity("Alice", "818727");
                var bob = new SubjectEntity("Bob", "88421113");

                db.Add(alice);
                db.Add(bob);

                await db.SaveChangesAsync();

                var application = new ApplicationEntity(BaleaConstants.DefaultApplicationName, "Default application");
                var viewGradesPermission = new PermissionEntity(Policies.GradesRead);
                var editGradesPermission = new PermissionEntity(Policies.GradesEdit);
                application.Permissions.Add(viewGradesPermission);
                application.Permissions.Add(editGradesPermission);
                var teacherRole = new RoleEntity(nameof(Roles.Teacher), "Teacher role");
                teacherRole.Subjects.Add(new RoleSubjectEntity { SubjectId = alice.Id });
                teacherRole.Permissions.Add(new RolePermissionEntity { Permission = viewGradesPermission });
                teacherRole.Permissions.Add(new RolePermissionEntity { Permission = editGradesPermission });
                application.Roles.Add(teacherRole);
                application.Delegations.Add(new DelegationEntity(alice.Id, bob.Id, DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddYears(1), true));
                var studentRole = new RoleEntity(nameof(Roles.Student), "Student role");
                var mapping = new MappingEntity("customer");
                studentRole.Mappings.Add(new RoleMappingEntity { Mapping = mapping });
                application.Roles.Add(studentRole);
                db.Applications.Add(application);
                await db.SaveChangesAsync();
            }
        }
    }
}
