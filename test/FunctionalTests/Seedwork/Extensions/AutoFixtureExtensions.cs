using System.Collections.Generic;
using System.Security.Claims;

namespace AutoFixture
{
    public static class AutoFixtureExtensions
    {
        public const string TeacherSub = "1";
        public const string FirstSubstituteSub = "2";
        public const string SecondSubstituteSub = "3";
        public const string CustodianSub = "4";

        public static IEnumerable<Claim> Teacher(this Fixture fixture)
        {
            return GetClaims(fixture, new Claim(Balea.BaleaClaims.Subject, TeacherSub));
        }

        public static IEnumerable<Claim> Custodian(this Fixture fixture)
        {
            return GetClaims(fixture, new Claim(Balea.BaleaClaims.Subject, CustodianSub));
        }

        public static IEnumerable<Claim> FirstSubstitute(this Fixture fixture)
        {
            return GetClaims(fixture, new Claim(Balea.BaleaClaims.Subject, FirstSubstituteSub));
        }

        public static IEnumerable<Claim> SecondSubstitute(this Fixture fixture)
        {
            return GetClaims(fixture, new Claim(Balea.BaleaClaims.Subject, SecondSubstituteSub));
        }

        private static IEnumerable<Claim> GetClaims(Fixture fixture, params Claim [] claims)
        {
            return fixture.Build<Claim[]>().FromFactory(() => claims).Create();
        }
    }
}
