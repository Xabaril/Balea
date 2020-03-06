using Balea.EntityFrameworkCore.Store.Entities;
using Balea.EntityFrameworkCore.Store.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Balea.EntityFrameworkCore.Store.EntityConfigurations
{
    internal class RoleEntityConfiguration : IEntityTypeConfiguration<RoleEntity>
    {
        private readonly StoreOptions _options;

        public RoleEntityConfiguration(StoreOptions options)
        {
            _options = options ?? throw new System.ArgumentNullException(nameof(options));
        }

        public void Configure(EntityTypeBuilder<RoleEntity> builder)
        {
            builder.ToTable(_options.Roles.Name);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();
            builder
                .HasIndex(x => x.Name)
                .IsUnique();
            builder.Property(x => x.Description)
                .HasMaxLength(500)
                .IsRequired(false);
            builder.Property(x => x.Enabled)
                .HasDefaultValue(true)
                .IsRequired();
        }
    }
}
