using System.Linq;
using Volvoreta.EntityFrameworkCore.Store.DbContexts;
using Volvoreta.EntityFrameworkCore.Store.Entities;

namespace WebApp.Infrastratucture.Data.Seeders
{
    public static class VolvoretaSeeder
    {
        public static void Seed(StoreDbContext db)
        {
            if (!db.Roles.Any())
            {
                var teacherRole = new RoleEntity
                {
                    Name = "teacher",
                    Description = "Teacher role",
                    Enabled = true,
                };

                var johnSmith = new SubjectEntity
                {
                    Sub = "10000",
                    Name = "John Smith"
                };

                var roleSubject = new RoleSubjectEntity
                {
                    Role = teacherRole,
                    Subject = johnSmith
                };

                var editGrades = new PermissionEntity
                {
                    Name = "edit.grades"
                };

                var viewGrades = new PermissionEntity
                {
                    Name = "view.grades"
                };

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
