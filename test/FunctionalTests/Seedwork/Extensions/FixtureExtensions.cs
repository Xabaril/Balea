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
                var john = new SubjectEntity
                {
                    Name = "John",
                    Sub = AutoFixtureExtensions.TeacherSub
                };
                var mary = new SubjectEntity
                {
                    Name = "Mary",
                    Sub = AutoFixtureExtensions.FirstSubstituteSub
                };
                var anna = new SubjectEntity
                {
                    Name = "Anna",
                    Sub = AutoFixtureExtensions.SecondSubstituteSub
                };

                db.Add(john);
                db.Add(mary);
                db.Add(anna);

                await db.SaveChangesAsync();

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
                                    SubjectId = john.Id
                                }
                            }
                        }
                    },
                    Delegations = new DelegationEntity[]
                    {
                        new DelegationEntity
                        {
                            WhoId = john.Id,
                            WhomId = mary.Id,
                            From = DateTime.UtcNow.AddDays(-1),
                            To = DateTime.UtcNow.AddDays(1),
                            Selected = selectedDelegation
                        },
                        new DelegationEntity
                        {
                            WhoId = john.Id,
                            WhomId = anna.Id,
                            From = DateTime.UtcNow.AddDays(-1),
                            To = DateTime.UtcNow.AddDays(1),
                            Selected = selectedDelegation
                        }
                    }
                };

                db.Applications.Add(application);
                await db.SaveChangesAsync();
            });
        }
    }
}
