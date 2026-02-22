using DotNet.Testcontainers.Builders;
using Testcontainers.PostgreSql;
using Xunit;

namespace QuestionManagementService.Tests.Integration.TestInfrastructure;

public class DatabaseFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:18.1")
        .WithDatabase("testsdelivery_questions_test")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432))
        .Build();

    public PostgreSqlContainer DbContainer => _dbContainer;

    public async Task InitializeAsync()
    {
        await DbContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await DbContainer.DisposeAsync();
    }
}

[CollectionDefinition("Database")]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
{
}
