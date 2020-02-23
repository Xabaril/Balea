using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using Balea.EntityFrameworkCore.Store.Entities;
using Balea.EntityFrameworkCore.Store.Options;

namespace Balea.EntityFrameworkCore.Store.EntityConfigurations
{
    internal class RolePermissionEntityConfiguration : IEntityTypeConfiguration<RolePermissionEntity>
    {
        private readonly StoreOptions _options;

        public RolePermissionEntityConfiguration(StoreOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public void Configure(EntityTypeBuilder<RolePermissionEntity> builder)
        {
            builder.ToTable($"{_options.Roles.Name}{_options.Permissions.Name}");
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
