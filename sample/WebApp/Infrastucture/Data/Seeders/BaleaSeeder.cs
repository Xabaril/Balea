using System.Linq;
using Balea.EntityFrameworkCore.Store.DbContexts;
using Balea.EntityFrameworkCore.Store.Entities;

namespace WebApp.Infrastucture.Data.Seeders
{
    public static class BaleaSeeder
    {
        public static void Seed(StoreDbContext db)
        {
            if (!db.Roles.Any())
            {
                var teacherRole = new RoleEntity("teacher", "Teacher role");

                var johnSmith = new SubjectEntity("John Smith", "10000");

                var roleSubject = new RoleSubjectEntity
                {
                    Role = teacherRole,
                    Subject = johnSmith
                };

                var editGrades = new PermissionEntity("edit.grades");
                var viewGrades = new PermissionEntity("view.grades");

                var viewGradesRolePermission = new RolePermissionEntity
                {
                    Role = teacherRole,
                    Permission = viewGrades
                };

                var editGradesRolePermission = new RolePermissionEntity
                {
                    Role = teacherRole,
                    Permission = editGrades
                };

                db.Roles.Add(teacherRole);
                db.Subjects.Add(johnSmith);
                db.RoleSubjects.Add(roleSubject);
                db.Permissions.Add(editGrades);
                db.Permissions.Add(viewGrades);
                db.RolePermissions.Add(viewGradesRolePermission);
                db.RolePermissions.Add(editGradesRolePermission);

                db.SaveChanges();
            }
        }
    }
}
