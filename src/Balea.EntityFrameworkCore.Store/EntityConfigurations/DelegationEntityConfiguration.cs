using Balea.EntityFrameworkCore.Store.Entities;
using Balea.EntityFrameworkCore.Store.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Balea.EntityFrameworkCore.Store.EntityConfigurations
{
    internal class DelegationEntityConfiguration : IEntityTypeConfiguration<DelegationEntity>
    {
        private readonly StoreOptions _options;

        public DelegationEntityConfiguration(StoreOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public void Configure(EntityTypeBuilder<DelegationEntity> builder)
        {
            builder.ToTable(_options.Delegations.Name);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.From)
                .IsRequired();
            builder.Property(x => x.To)
                .IsRequired();
            builder
                .HasOne(x => x.Who)
                .WithMany(x => x.WhoDelegations)
                .HasForeignKey(x => x.WhoId)
                .OnDelete(DeleteBehavior.NoAction);
            builder
                .HasOne(x => x.Whom)
                .WithMany(x => x.WhomDelegations)
                .HasForeignKey(x => x.WhomId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
