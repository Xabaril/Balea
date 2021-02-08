namespace Balea.Authorization.Abac
{
    public class AbacPrefix
    {
        private const string Prefix = "abac__";

        public AbacPrefix(string policyName)
        {
            PolicyName = policyName.Replace(Prefix, string.Empty);
        }

        public string PolicyName { get; }

        public override string ToString()
        {
            return $"{Prefix}{PolicyName}";
        }
    }
}
