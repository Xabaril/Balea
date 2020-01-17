using System.Collections.Generic;

namespace Volvoreta.EntityFrameworkCore.Store.Entities
{
    public class PermissionEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<RolePermissionEntity> Roles { get; set; }
    }
}
