namespace Lagkage.Integration.Tests;

[CollectionDefinition("Database collection")]
public class DatabaseTestCollection : ICollectionFixture<DatabaseFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces
}