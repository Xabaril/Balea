using Microsoft.EntityFrameworkCore;
using System;

namespace Volvoreta.EntityFrameworkCore.Store.Options
{
    /// <summary>
    /// Provide programatically configuration for <see cref="StoreDbContext"/>.
    /// </summary>
    public class StoreOptions
    {
        public Action<DbContextOptionsBuilder> ConfigureDbContext { get; set; }

        /// <summary>
        /// Get or set a new action for add new configuration for <see cref="DbContextOptionsBuilder"/>
        /// </summary>
        public Action<IServiceProvider, DbContextOptionsBuilder> ResolveDbContextOptions { get; set; }

        /// <summary>
        /// Get or set default schema for store configuration tables.
        /// </summary>
        public string DefaultSchema { get; set; } = null;

        /// <summary>
        /// Get or set default table configuration for Roles.
        /// </summary>
        public TableConfiguration Roles { get; set; } = new TableConfiguration(nameof(Roles));

        /// <summary>
        /// Get or set default table configuration for Mappings.
        /// </summary>
        public TableConfiguration Mappings { get; set; } = new TableConfiguration(nameof(Mappings));

        /// <summary>
        /// Get or set default table configuration for Permissions.
        /// </summary>
        public TableConfiguration Permissions { get; set; } = new TableConfiguration(nameof(Permissions));

        /// <summary>
        /// Get or set default table configuration for Subject Id's.
        /// </summary>
        public TableConfiguration Subjects { get; set; } = new TableConfiguration(nameof(Subjects));
    }
}
