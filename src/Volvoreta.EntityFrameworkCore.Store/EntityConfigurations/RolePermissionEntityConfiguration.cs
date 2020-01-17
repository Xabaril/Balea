using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using Volvoreta.EntityFrameworkCore.Store.Entities;
using Volvoreta.EntityFrameworkCore.Store.Options;

namespace Volvoreta.EntityFrameworkCore.Store.EntityConfigurations
{
    internal class RolePermissionEntityConfiguration : IEntityTypeConfiguration<RolePermissionEntity>
    {
        private readonly StoreOptions options;

        public RolePermissionEntityConfiguration(StoreOptions options)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public void Configure(EntityTypeBuilder<RolePermissionEntity> builder)
        {
            builder.ToTable($"{options.Roles.Name}{options.Permissions.Name}");
            builder.HasKey(x => new { x.RoleId, x.PermissionId });
            builder
                .HasOne(x => x.Permission)
                .WithMany(x => x.Roles)
                .HasForeignKey(x => x.PermissionId);
            builder
                .HasOne(x => x.Role)
                .WithMany(x => x.Permissions)
                .HasForeignKey(x => x.RoleId);
        }
    }
}
