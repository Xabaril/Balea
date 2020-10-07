namespace Balea.EntityFrameworkCore.Store.Entities
{
    public class RoleTagEntity
    {
        public int RoleId { get; set; }
        public RoleEntity Role { get; set; }
        public int TagId { get; set; }
        public TagEntity Tag { get; set; }
    }
}
