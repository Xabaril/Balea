using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using Volvoreta.EntityFrameworkCore.Store.Entities;
using Volvoreta.EntityFrameworkCore.Store.Options;

namespace Volvoreta.EntityFrameworkCore.Store.EntityConfigurations
{
    internal class RoleSubjectsEntityConfiguration : IEntityTypeConfiguration<RoleSubjectEntity>
    {
        private readonly StoreOptions options;

        public RoleSubjectsEntityConfiguration(StoreOptions options)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public void Configure(EntityTypeBuilder<RoleSubjectEntity> builder)
        {
            builder.ToTable($"{options.Roles.Name}{options.Subjects.Name}");
            builder.HasKey(x => new { x.RoleId, x.SubjectId });
            builder
                .HasOne(x => x.Subject)
                .WithMany(x => x.Roles)
                .HasForeignKey(x => x.SubjectId);
            builder
                .HasOne(x => x.Role)
                .WithMany(x => x.Subjects)
                .HasForeignKey(x => x.RoleId);
        }
    }
}
