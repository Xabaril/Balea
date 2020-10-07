using Balea.EntityFrameworkCore.Store.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Balea.EntityFrameworkCore.Store.EntityConfigurations
{
    internal class RoleTagEntityConfiguration : IEntityTypeConfiguration<RoleTagEntity>
    {
        public void Configure(EntityTypeBuilder<RoleTagEntity> builder)
        {
            builder.HasKey(x => new { x.RoleId, x.TagId });

            builder
                .HasOne(x => x.Tag)
                .WithMany(x => x.Roles)
                .HasForeignKey(x => x.TagId);

            builder
                .HasOne(x => x.Role)
                .WithMany(x => x.Tags)
                .HasForeignKey(x => x.RoleId);
        }
    }
}
