using Balea.EntityFrameworkCore.Store.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Balea.EntityFrameworkCore.Store.EntityConfigurations
{
    internal class RoleSubjectsEntityConfiguration : IEntityTypeConfiguration<RoleSubjectEntity>
    {
        public void Configure(EntityTypeBuilder<RoleSubjectEntity> builder)
        {
            builder.HasKey(x => new { x.RoleId, x.SubjectId });
            builder
                .HasOne(x => x.Subject)
                .WithMany(x => x.Roles)
                .HasForeignKey(x => x.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasOne(x => x.Role)
                .WithMany(x => x.Subjects)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
