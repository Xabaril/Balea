using System.Collections.Generic;

namespace Balea.EntityFrameworkCore.Store.Entities
{
    public class ApplicationEntity
    {
        public ApplicationEntity(string name, string description = null)
        {
            Name = name;
            Description = description;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<RoleEntity> Roles { get; set; } = new List<RoleEntity>();
        public ICollection<DelegationEntity> Delegations { get; set; } = new List<DelegationEntity>();
        public ICollection<PermissionEntity> Permissions { get; set; } = new List<PermissionEntity>();
    }
}
