using System.Collections.Generic;

namespace Volvoreta.EntityFrameworkCore.Store.Entities
{
    public class ApplicationEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<RoleEntity> Roles { get; set; }
        public ICollection<DelegationEntity> Delegations { get; set; }
    }
}
