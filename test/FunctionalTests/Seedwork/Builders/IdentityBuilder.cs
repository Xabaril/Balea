using System.Collections.Generic;
using System.Security.Claims;

namespace FunctionalTests.Seedwork.Builders
{
    public class IdentityBuilder
    {
        private const string TeacherSub = "1";
        private const string CustodianSub = "3";
        private string sub = "-1";

        public IdentityBuilder WithSub(string sub)
        {
            this.sub = sub;
            return this;
        }

        public List<Claim> Teacher()
        {
            return new List<Claim>()
            {
                new Claim(Volvoreta.Claims.Subject, TeacherSub)
            };
        }

        public List<Claim> Custodian()
        {
            return new List<Claim>()
            {
                new Claim(Volvoreta.Claims.Subject, CustodianSub)
            };
        }

        public List<Claim> Build()
        {
            return new List<Claim>()
            {
                new Claim(Volvoreta.Claims.Subject, sub)
            };
        }
    }
}
