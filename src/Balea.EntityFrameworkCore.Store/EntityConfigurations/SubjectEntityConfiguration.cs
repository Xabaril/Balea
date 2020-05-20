using Balea.EntityFrameworkCore.Store.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Balea.EntityFrameworkCore.Store.EntityConfigurations
{
    internal class SubjectEntityConfiguration : IEntityTypeConfiguration<SubjectEntity>
    {
        public void Configure(EntityTypeBuilder<SubjectEntity> builder)
        {
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
            builder.Property(x => x.ImageUrl)
                .HasMaxLength(2048)
                .IsRequired(false);
        }
    }
}
