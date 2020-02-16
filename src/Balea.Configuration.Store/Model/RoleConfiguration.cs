namespace Balea.Configuration.Store.Model
{
    public class RoleConfiguration
    {
        public string Name { get; set; }
        public bool Enabled { get; set; }
        public string Description { get; set; }
        public string[] Permissions { get; set; } = new string[0];
        public string[] Subjects { get; set; } = new string[0];
        public string[] Mappings { get; set; } = new string[0];
    }
}
