namespace Volvoreta.EntityFrameworkCore.Store.Entities
{
    public class RoleSubjectEntity
    {
        public int RoleId { get; set; }
        public RoleEntity Role { get; set; }
        public int SubjectId { get; set; }
        public SubjectEntity Subject { get; set; }
    }
}
