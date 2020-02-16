using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using Balea.EntityFrameworkCore.Store.Entities;
using Balea.EntityFrameworkCore.Store.Options;

namespace Balea.EntityFrameworkCore.Store.EntityConfigurations
{
    internal class PermissionEntityConfiguration : IEntityTypeConfiguration<PermissionEntity>
    {
        private readonly StoreOptions options;

        public PermissionEntityConfiguration(StoreOptions options)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public void Configure(EntityTypeBuilder<PermissionEntity> builder)
        {
            builder.ToTable(options.Permissions.Name);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(x => x.Description)
                .HasMaxLength(500)
                .IsRequired(false);
        }
    }
}
