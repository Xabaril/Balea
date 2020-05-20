using Balea;
using System.Collections.Generic;
using System.Security.Claims;

namespace AutoFixture
{
    public static class AutoFixtureExtensions
    {
        public const string Subject = "sub";

        public static IEnumerable<Claim> Sub(this Fixture fixture, string sub)
        {
            return GetClaims(fixture, new Claim(Subject, sub));
        }

        public static IEnumerable<Claim> Client(this Fixture fixture, string sub)
        {
            return GetClaims(fixture, new Claim(JwtClaimTypes.ClientId, sub));
        }

        private static IEnumerable<Claim> GetClaims(Fixture fixture, params Claim[] claims)
        {
            return fixture.Build<Claim[]>().FromFactory(() => claims).Create();
        }
    }
}
