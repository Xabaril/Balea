using Microsoft.EntityFrameworkCore;
using Volvoreta.EntityFrameworkCore.Store.Entities;
using Volvoreta.EntityFrameworkCore.Store.Options;

namespace Volvoreta.EntityFrameworkCore.Store.DbContexts
{
    public class StoreDbContext : DbContext
    {
        private readonly StoreOptions storeOptions;

        public DbSet<ApplicationEntity> Applications { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<MappingEntity> Mappings { get; set; }
        public DbSet<PermissionEntity> Permissions { get; set; }
        public DbSet<SubjectEntity> Subjects { get; set; }
        public DbSet<DelegationEntity> Delegations { get; set; }
        public DbSet<RolePermissionEntity> RolePermissions { get; set; }
        public DbSet<RoleMappingEntity> RoleMappings { get; set; }
        public DbSet<RoleSubjectEntity> RoleSubjects { get; set; }

        public StoreDbContext(DbContextOptions<StoreDbContext> options)
            : this(options, new StoreOptions())
        {

        }

        public StoreDbContext(DbContextOptions<StoreDbContext> options, StoreOptions storeOptions)
            : base (options)
        {
            this.storeOptions = storeOptions;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(StoreDbContext).Assembly, storeOptions);

            base.OnModelCreating(modelBuilder);
        }
    }
}
