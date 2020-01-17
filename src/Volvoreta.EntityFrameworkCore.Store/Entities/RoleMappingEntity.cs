namespace Volvoreta.EntityFrameworkCore.Store.Entities
{
    public class RoleMappingEntity
    {
        public int RoleId { get; set; }
        public RoleEntity Role { get; set; }
        public int MappingId { get; set; }
        public MappingEntity Mapping { get; set; }
    }
}
