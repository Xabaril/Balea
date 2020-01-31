using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using Volvoreta.EntityFrameworkCore.Store.Entities;
using Volvoreta.EntityFrameworkCore.Store.Options;

namespace Volvoreta.EntityFrameworkCore.Store.EntityConfigurations
{
    internal class ApplicationEntityConfiguration : IEntityTypeConfiguration<ApplicationEntity>
    {
        private readonly StoreOptions options;

        public ApplicationEntityConfiguration(StoreOptions options)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public void Configure(EntityTypeBuilder<ApplicationEntity> builder)
        {
            builder.ToTable(options.Applications.Name);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(x => x.Description)
                .HasMaxLength(500)
                .IsRequired(false);
            builder
                .HasIndex(x => x.Name)
                .IsUnique();
        }
    }
}
