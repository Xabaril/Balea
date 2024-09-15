namespace Balea.Model
{
    public class Policy
    {
        public Policy(string name, string content)
        {
            Name = name;
            Content = content;
        }

        public string Name { get; private set; }
        public string Content { get; private set; }
    }
}
