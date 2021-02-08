using Balea.Model;

namespace Balea.Api.Store.Model
{
    public class HttpClientStorePolicyResponse
    {
        public string Name { get; set; }
        public string Content { get; set; }

        public Policy To()
        {
            return new Policy(Name, Content);
        }
    }
}
