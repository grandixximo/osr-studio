using Xunit;

namespace OsrStudio.Tests.Fixtures
{
    [CollectionDefinition(nameof(Tests))]
    public class TestCollection : ICollectionFixture<TestManagerFixture>, ICollectionFixture<MoqFixture> { }
}