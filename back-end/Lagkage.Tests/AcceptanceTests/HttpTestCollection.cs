using Lagkage.Contracts.Http;

namespace Lagkage.Integration.Tests.AcceptanceTests;

[CollectionDefinition("Http collection")]
public class DatabaseTestCollection : ICollectionFixture<HttpFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces
}