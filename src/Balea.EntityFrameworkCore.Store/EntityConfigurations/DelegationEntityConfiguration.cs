using Balea.EntityFrameworkCore.Store.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Balea.EntityFrameworkCore.Store.EntityConfigurations
{
    internal class DelegationEntityConfiguration : IEntityTypeConfiguration<DelegationEntity>
    {
        public void Configure(EntityTypeBuilder<DelegationEntity> builder)
        {
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
