using System.Reflection;
using Xunit.Sdk;

namespace FunctionalTests.Seedwork
{
    public class ResetDatabaseAttribute
        : BeforeAfterTestAttribute
    {
        public override void Before(MethodInfo methodUnderTest)
        {
            TestServerFixture.ResetDatabase().Wait();
        }
    }
}
