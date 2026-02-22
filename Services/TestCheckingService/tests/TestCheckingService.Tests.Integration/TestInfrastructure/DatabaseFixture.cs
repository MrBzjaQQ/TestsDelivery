using Microsoft.EntityFrameworkCore;
using TestCheckingService.Infrastructure.Database.Context;
using Testcontainers.PostgreSql;
using Xunit;

namespace TestCheckingService.Tests.Integration.TestInfrastructure;

public class DatabaseFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = CreateDbContainer();

    public PostgreSqlContainer DbContainer => _dbContainer;

    public async Task InitializeAsync()
    {
        await DbContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await DbContainer.DisposeAsync();
    }

    private static PostgreSqlContainer CreateDbContainer()
    {
        return new PostgreSqlBuilder()
            .WithImage("postgres:18.1")
            .Build();
    }
}

[CollectionDefinition("Database")]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
{
}
