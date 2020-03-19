namespace Balea.Configuration.Store.Model
{
    public class ApplicationConfiguration
    {
        public string Name { get; set; }
        public RoleConfiguration[] Roles { get; set; } = new RoleConfiguration[0];
        public DelegationConfiguration[] Delegations { get; set; } = new DelegationConfiguration[0];
;    }
}