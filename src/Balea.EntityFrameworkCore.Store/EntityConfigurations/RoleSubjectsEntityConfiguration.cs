using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using Balea.EntityFrameworkCore.Store.Entities;
using Balea.EntityFrameworkCore.Store.Options;

namespace Balea.EntityFrameworkCore.Store.EntityConfigurations
{
    internal class RoleSubjectsEntityConfiguration : IEntityTypeConfiguration<RoleSubjectEntity>
    {
        private readonly StoreOptions _options;

        public RoleSubjectsEntityConfiguration(StoreOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public void Configure(EntityTypeBuilder<RoleSubjectEntity> builder)
        {
            builder.ToTable($"{_options.Roles.Name}{_options.Subjects.Name}");
            builder.HasKey(x => new { x.RoleId, x.SubjectId });
            builder
                .HasOne(x => x.Subject)
                .WithMany(x => x.Roles)
                .HasForeignKey(x => x.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasOne(x => x.Role)
                .WithMany(x => x.Subjects)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
