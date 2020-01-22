using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using Volvoreta.EntityFrameworkCore.Store.Entities;
using Volvoreta.EntityFrameworkCore.Store.Options;

namespace Volvoreta.EntityFrameworkCore.Store.EntityConfigurations
{
    internal class DelegationEntityConfiguration : IEntityTypeConfiguration<DelegationEntity>
    {
        private readonly StoreOptions options;

        public DelegationEntityConfiguration(StoreOptions options)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public void Configure(EntityTypeBuilder<DelegationEntity> builder)
        {
            builder.ToTable(options.Delegations.Name);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Who)
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(x => x.Whom)
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(x => x.From)
                .IsRequired();
            builder.Property(x => x.To)
                .IsRequired();
            builder
                .HasIndex(x => new { x.Who, x.Whom })
                .IsUnique();
        }
    }
}
