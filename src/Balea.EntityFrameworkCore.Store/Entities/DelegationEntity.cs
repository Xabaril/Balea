using System;

namespace Balea.EntityFrameworkCore.Store.Entities
{
    public class DelegationEntity
    {
        public DelegationEntity(int whoId, int whomId, DateTime from, DateTime to, bool selected)
        {
            WhoId = whoId;
            WhomId = whomId;
            From = from;
            To = to;
            Selected = selected;
        }

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
