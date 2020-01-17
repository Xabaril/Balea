using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volvoreta.EntityFrameworkCore.Store.Entities;
using Volvoreta.EntityFrameworkCore.Store.Options;

namespace Volvoreta.EntityFrameworkCore.Store.EntityConfigurations
{
    internal class RoleEntityConfiguration : IEntityTypeConfiguration<RoleEntity>
    {
        private readonly StoreOptions options;

        public RoleEntityConfiguration(StoreOptions options)
        {
            this.options = options ?? throw new System.ArgumentNullException(nameof(options));
        }

        public void Configure(EntityTypeBuilder<RoleEntity> builder)
        {
            builder.ToTable(options.Roles.Name);
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
