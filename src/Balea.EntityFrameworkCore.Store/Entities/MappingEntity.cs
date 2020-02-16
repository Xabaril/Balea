using System.Collections.Generic;

namespace Balea.EntityFrameworkCore.Store.Entities
{
    public class MappingEntity
    {
        public MappingEntity(string name)
        {
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<RoleMappingEntity> Roles { get; set; }
    }
}
