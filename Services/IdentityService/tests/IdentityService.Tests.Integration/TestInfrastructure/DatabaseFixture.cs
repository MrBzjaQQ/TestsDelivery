using Testcontainers.PostgreSql;
using Xunit;

namespace IdentityService.Tests.Integration.TestInfrastructure;

[CollectionDefinition("Database")]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
{
}

public class DatabaseFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:16")
        .WithDatabase("testsdelivery_identity_test")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    public PostgreSqlContainer DbContainer => _dbContainer;

    public async Task InitializeAsync() => await _dbContainer.StartAsync();

    public async Task DisposeAsync() => await _dbContainer.DisposeAsync();
}
