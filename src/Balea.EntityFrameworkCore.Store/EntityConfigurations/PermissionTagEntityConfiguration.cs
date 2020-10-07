using Balea.EntityFrameworkCore.Store.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Balea.EntityFrameworkCore.Store.EntityConfigurations
{
    internal class PermissionTagEntityConfiguration : IEntityTypeConfiguration<PermissionTagEntity>
    {
        public void Configure(EntityTypeBuilder<PermissionTagEntity> builder)
        {
            builder.HasKey(x => new { x.PermissionId, x.TagId });

            builder
                .HasOne(x => x.Tag)
                .WithMany(x => x.Permissions)
                .HasForeignKey(x => x.TagId);

            builder
                .HasOne(x => x.Permission)
                .WithMany(x => x.Tags)
                .HasForeignKey(x => x.PermissionId);
        }
    }
}
