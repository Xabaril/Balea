namespace Balea.EntityFrameworkCore.Store.Entities
{
    public class PermissionTagEntity
    {
        public int PermissionId { get; set; }
        public PermissionEntity Permission { get; set; }
        public int TagId { get; set; }
        public TagEntity Tag { get; set; }
    }
}
