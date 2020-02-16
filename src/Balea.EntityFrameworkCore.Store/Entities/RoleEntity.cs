using System.Collections.Generic;

namespace Balea.EntityFrameworkCore.Store.Entities
{
    public class RoleEntity
    {
        public RoleEntity(string name, string description = null)
        {
            Name = name;
            Description = description;
            Enabled = true;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
        public int ApplicationId { get; set; }
        public ApplicationEntity Application { get; set; }
        public ICollection<RoleMappingEntity> Mappings { get; set; } = new List<RoleMappingEntity>();
        public ICollection<RoleSubjectEntity> Subjects { get; set; } = new List<RoleSubjectEntity>();
        public ICollection<RolePermissionEntity> Permissions { get; set; } = new List<RolePermissionEntity>();
    }
}
