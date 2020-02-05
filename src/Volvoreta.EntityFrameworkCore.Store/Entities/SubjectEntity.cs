using System.Collections.Generic;

namespace Volvoreta.EntityFrameworkCore.Store.Entities
{
    public class SubjectEntity
    {
        public int Id { get; set; }
        public string Sub { get; set; }
        public string Name { get; set; }
        public ICollection<RoleSubjectEntity> Roles { get; set; }
        public ICollection<DelegationEntity> WhoDelegations { get; set; }
        public ICollection<DelegationEntity> WhomDelegations { get; set; }
    }
}
