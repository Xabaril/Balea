using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using Balea.EntityFrameworkCore.Store.Entities;
using Balea.EntityFrameworkCore.Store.Options;

namespace Balea.EntityFrameworkCore.Store.EntityConfigurations
{
    internal class MappingEntityConfiguration : IEntityTypeConfiguration<MappingEntity>
    {
        private readonly StoreOptions _options;

        public MappingEntityConfiguration(StoreOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public void Configure(EntityTypeBuilder<MappingEntity> builder)
        {
            builder.ToTable(_options.Mappings.Name);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();
            builder
                .HasIndex(x => x.Name)
                .IsUnique();
        }
    }
}
