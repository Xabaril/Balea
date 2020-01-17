using System;

namespace Volvoreta.Configuration.Store.Model
{
    public class DelegationConfiguration
    {
        public string Who { get; set; }
        public string Whom { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public bool Selected { get; set; }
        public bool Active => Selected && From <= DateTime.UtcNow && To >= DateTime.UtcNow;
    }
}
