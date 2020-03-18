using Balea.EntityFrameworkCore.Store.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Balea.EntityFrameworkCore.Store.EntityConfigurations
{
    internal class ApplicationEntityConfiguration : IEntityTypeConfiguration<ApplicationEntity>
    {
        public void Configure(EntityTypeBuilder<ApplicationEntity> builder)
        {
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
