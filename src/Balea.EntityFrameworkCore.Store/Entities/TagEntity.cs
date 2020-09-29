using System.Collections.Generic;

namespace Balea.EntityFrameworkCore.Store.Entities
{
    public class TagEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<PermissionTagEntity> Permissions { get; set; } = new List<PermissionTagEntity>();
        public ICollection<RoleTagEntity> Roles { get; set; } = new List<RoleTagEntity>();
    }
}
