using System;

namespace Volvoreta.EntityFrameworkCore.Store.Entities
{
    public class DelegationEntity
    {
        public int Id { get; set; }
        public int WhoId { get; set; }
        public SubjectEntity Who { get; set; }
        public int WhomId { get; set; }
        public SubjectEntity Whom { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public bool Selected { get; set; }
    }
}
