using Xunit;

namespace FunctionalTests.Seedwork
{
    [CollectionDefinition(nameof(TestServerCollectionFixture))]
    public class TestServerCollectionFixture : ICollectionFixture<TestServerFixture>
    {

    }
}
