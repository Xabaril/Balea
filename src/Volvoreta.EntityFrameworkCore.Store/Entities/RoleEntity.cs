using System.Collections.Generic;

namespace Volvoreta.EntityFrameworkCore.Store.Entities
{
    public class RoleEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
        public int ApplicationId { get; set; }
        public ApplicationEntity Application { get; set; }
        public ICollection<RoleMappingEntity> Mappings { get; set; }
        public ICollection<RoleSubjectEntity> Subjects { get; set; }
        public ICollection<RolePermissionEntity> Permissions { get; set; }
    }
}
