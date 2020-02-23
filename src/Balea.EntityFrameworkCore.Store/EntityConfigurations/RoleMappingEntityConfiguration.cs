using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using Balea.EntityFrameworkCore.Store.Entities;
using Balea.EntityFrameworkCore.Store.Options;

namespace Balea.EntityFrameworkCore.Store.EntityConfigurations
{
    internal class RoleMappingEntityConfiguration : IEntityTypeConfiguration<RoleMappingEntity>
    {
        private readonly StoreOptions _options;

        public RoleMappingEntityConfiguration(StoreOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public void Configure(EntityTypeBuilder<RoleMappingEntity> builder)
        {
            builder.ToTable($"{_options.Roles.Name}{_options.Mappings.Name}");
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
