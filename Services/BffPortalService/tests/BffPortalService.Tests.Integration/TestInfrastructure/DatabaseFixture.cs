using Testcontainers.PostgreSql;
using Xunit;

namespace BffPortalService.Tests.Integration.TestInfrastructure;

public class DatabaseFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer;

    public DatabaseFixture()
    {
        _dbContainer = new PostgreSqlBuilder()
            .WithImage("postgres:18.1")
            .WithDatabase("testsdelivery_bff_test")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();
    }

    public string ConnectionString => _dbContainer.GetConnectionString();

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
    }
}

[CollectionDefinition("Database")]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
{
}
