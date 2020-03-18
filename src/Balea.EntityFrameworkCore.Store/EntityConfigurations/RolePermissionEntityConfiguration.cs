using Balea.EntityFrameworkCore.Store.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Balea.EntityFrameworkCore.Store.EntityConfigurations
{
    internal class RolePermissionEntityConfiguration : IEntityTypeConfiguration<RolePermissionEntity>
    {
        public void Configure(EntityTypeBuilder<RolePermissionEntity> builder)
        {
            builder.HasKey(x => new { x.RoleId, x.PermissionId });
            builder
                .HasOne(x => x.Permission)
                .WithMany(x => x.Roles)
                .HasForeignKey(x => x.PermissionId)
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasOne(x => x.Role)
                .WithMany(x => x.Permissions)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
