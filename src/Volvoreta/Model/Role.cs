using System.Collections.Generic;

namespace Volvoreta.Model
{
    public class Role
    {
        private readonly List<string> _subjects = new List<string>();
        private readonly List<string> _mappings = new List<string>();
        private readonly List<string> _permissions = new List<string>();

        public Role(
            string name, 
            string description,
            IEnumerable<string> subjects,
            IEnumerable<string> mappings,
            IEnumerable<string> permissions,
            bool enabled = true)
        {
            Name = name;
            Description = description;
            Enabled = enabled;
            _subjects.AddRange(subjects);
            _mappings.AddRange(mappings);
            _permissions.AddRange(permissions);
        }

        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool Enabled { get; private set; }

        public void AddSubjects(IEnumerable<string> subjects)
        {
            _subjects.AddRange(subjects);
        }

        public void AddMappings(IEnumerable<string> mappings)
        {
            _mappings.AddRange(mappings);
        }

        public void AddPermissions(IEnumerable<string> permissions)
        {
            _permissions.AddRange(permissions);
        }

        public IEnumerable<string> GetPermissions()
        {
            return _permissions;
        }

        public IEnumerable<string> GetMappings()
        {
            return _mappings;
        }

        public IEnumerable<string> GetSubjects()
        {
            return _subjects;
        }
    }
}
