namespace Balea.Authorization.Abac
{
    internal class AbacPrefix
    {
        private const string Prefix = "abac__";

        public AbacPrefix(string policy)
        {
            Policy = policy.Replace(Prefix, string.Empty);
        }

        public string Policy { get; }

        public override string ToString()
        {
            return $"{Prefix}{Policy}";
        }
    }
}
