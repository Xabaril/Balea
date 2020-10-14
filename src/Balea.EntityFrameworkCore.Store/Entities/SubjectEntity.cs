using System.Collections.Generic;

namespace Balea.EntityFrameworkCore.Store.Entities
{
    public class SubjectEntity
    {
        public SubjectEntity(string name, string sub, string imageUrl = null, string email = null)
        {
            Sub = sub;
            ImageUrl = imageUrl;
            Name = name;
            Email = email;
        }

        public int Id { get; set; }
        public string Sub { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Email { get; set; }
        public ICollection<RoleSubjectEntity> Roles { get; set; }
        public ICollection<DelegationEntity> WhoDelegations { get; set; }
        public ICollection<DelegationEntity> WhomDelegations { get; set; }
    }
}
