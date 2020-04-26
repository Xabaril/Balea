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

        public DelegationEntity(
            int whoId,
            int whomId,
            DateTime from,
            DateTime to,
            bool selected, int applicationId) : this(whoId, whomId,from, to, selected)
        {
            ApplicationId = applicationId;
        }

        public int Id { get; set; }
        public int WhoId { get; set; }
        public SubjectEntity Who { get; set; }
        public int WhomId { get; set; }
        public SubjectEntity Whom { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public bool Selected { get; set; }
        public int ApplicationId { get; set; }
        public ApplicationEntity Application { get; set; }
    }
}
