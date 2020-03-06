using Balea.EntityFrameworkCore.Store.Entities;
using Balea.EntityFrameworkCore.Store.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Balea.EntityFrameworkCore.Store.EntityConfigurations
{
    internal class SubjectEntityConfiguration : IEntityTypeConfiguration<SubjectEntity>
    {
        private readonly StoreOptions _options;

        public SubjectEntityConfiguration(StoreOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public void Configure(EntityTypeBuilder<SubjectEntity> builder)
        {
            builder.ToTable(_options.Subjects.Name);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Sub)
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();
            builder
                .HasIndex(x => x.Sub)
                .IsUnique();
        }
    }
}
