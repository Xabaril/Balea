using Balea.EntityFrameworkCore.Store.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Balea.EntityFrameworkCore.Store.EntityConfigurations
{
    internal class RoleMappingEntityConfiguration : IEntityTypeConfiguration<RoleMappingEntity>
    {
        public void Configure(EntityTypeBuilder<RoleMappingEntity> builder)
        {
            builder.HasKey(x => new { x.RoleId, x.MappingId });
            builder
                .HasOne(x => x.Mapping)
                .WithMany(x => x.Roles)
                .HasForeignKey(x => x.MappingId)
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasOne(x => x.Role)
                .WithMany(x => x.Mappings)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
