using Balea.EntityFrameworkCore.Store.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Balea.EntityFrameworkCore.Store.EntityConfigurations
{
    internal class PolicyEntityConfiguration : IEntityTypeConfiguration<PolicyEntity>
    {
        public void Configure(EntityTypeBuilder<PolicyEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => new { x.Name, x.ApplicationId })
                .IsUnique();
            builder.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(x => x.Content)
                .HasMaxLength(4000)
                .IsRequired();
            builder
                .HasOne(x => x.Application)
                .WithMany(x => x.Policies)
                .HasForeignKey(x => x.ApplicationId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
