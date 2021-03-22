namespace Balea.EntityFrameworkCore.Store.Entities
{
    public class PolicyEntity
    {
        public PolicyEntity(string name, string content)
        {
            Name = name;
            Content = content;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public int ApplicationId { get; set; }
        public ApplicationEntity Application { get; set; }
    }
}
