using System;

namespace Volvoreta.Model
{
    public class Delegation
    {
        public Delegation(
            string who,
            string whom,
            DateTime from,
            DateTime to)
        {
            Ensure.Argument.NotNullOrEmpty(who);
            Ensure.Argument.NotNullOrEmpty(whom);
            Ensure.Argument.Is(from < to, "to should be greater than from");

            Who = who;
            Whom = whom;
            From = from;
            To = to;
        }

        public string Who { get; private set; }
        public string Whom { get; private set; }
        public DateTime From { get; private set; }
        public DateTime To { get; private set; }
    }
}
