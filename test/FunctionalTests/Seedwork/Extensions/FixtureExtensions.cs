using FunctionalTests.Seedwork.Builders;
using System.Threading.Tasks;
using Volvoreta;
using Volvoreta.EntityFrameworkCore.Store.Entities;

namespace FunctionalTests.Seedwork
{
    public static class FixtureExtensions
    {
        public static async Task GiveAnApplicationWithTeacherRole(this TestServerFixture fixture)
        {
            await fixture.ExecuteDbContextAsync(async db =>
            {
                var application = new ApplicationEntity
                {
                    Name = VolvoretaConstants.DefaultApplicationName,
                    Description = "Default application",
                    Roles = new RoleEntity[]
                    {
                        new RoleEntity
                        {
                            Name = "Teacher",
                            Description = "Teacher role",
                            Enabled = true,
                            Permissions = new RolePermissionEntity[]
                            {
                                new RolePermissionEntity
                                {
                                    Permission = new PermissionEntity
                                    {
                                        Name = Policies.ViewGrades,
                                    }
                                },
                                new RolePermissionEntity
                                {
                                    Permission = new PermissionEntity
                                    {
                                        Name = Policies.EditGrades
                                    }
                                }
                            },
                            Subjects = new RoleSubjectEntity[]
                            {
                                new RoleSubjectEntity
                                {
                                    Subject = new SubjectEntity
                                    {
                                        Name = "John",
                                        Sub = IdentityBuilder.TeacherSub
                                    }
                                }
                            }
                        }
                    }
                };

                db.Applications.Add(application);
                await db.SaveChangesAsync();
            });
        }
    }
}
