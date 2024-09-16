using System.Collections.Generic;

namespace Balea.Model
{
    public class Role
    {
        private readonly List<string> _permissions = new List<string>();

        public Role(
            string name,
            string description,
            IEnumerable<string> permissions)
        {
            Name = name;
            Description = description;
            _permissions.AddRange(permissions);
        }

        public string Name { get; private set; }
        public string Description { get; private set; }

        public void AddPermissions(IEnumerable<string> permissions)
        {
            _permissions.AddRange(permissions);
        }

        public IEnumerable<string> GetPermissions()
        {
            return _permissions;
        }
    }
}
