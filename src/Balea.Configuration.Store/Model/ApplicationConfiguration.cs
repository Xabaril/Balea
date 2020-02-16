namespace Balea.Configuration.Store.Model
{
    public class ApplicationConfiguration
    {
        public string Name { get; set; }
        public RoleConfiguration[] Roles { get; set; }
        public DelegationConfiguration[] Delegations { get; set; }
    }
}