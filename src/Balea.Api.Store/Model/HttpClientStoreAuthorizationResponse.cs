using Balea.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Balea.Api.Store.Model
{
    public class HttpClientStoreAuthorizationResponse
    {
        public HttpClientStoreAuthorizationResponse()
        {
        }

        public IEnumerable<RoleResponse> Roles { get; set; }
        public DelegationResponse Delegation { get; set; }

        public AuthorizationContext To()
        {
            return new AuthorizationContext(
                Roles.Select(role => new Role(
                    role.Name,
                    role.Description,
                    role.Subjects,
                    role.Mappings,
                    role.Permissions,
                    role.Enabled)),
                Delegation is null 
                    ? null 
                    : new Delegation(
                        Delegation.Who,
                        Delegation.Whom,
                        Delegation.From,
                        Delegation.To));
        }
    }

    public class RoleResponse
    {
        public RoleResponse()
        {

        }

        public string Name { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
        public IEnumerable<string> Permissions { get; set; }
        public IEnumerable<string> Mappings { get; set; }
        public IEnumerable<string> Subjects { get; set; }
    }

    public class DelegationResponse
    {
        public DelegationResponse()
        {

        }

        public string Who { get; set; }
        public string Whom { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
