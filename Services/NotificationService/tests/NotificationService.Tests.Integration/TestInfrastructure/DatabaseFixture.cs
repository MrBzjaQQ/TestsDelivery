using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using NotificationService.Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace NotificationService.Tests.Integration.TestInfrastructure;

public class DatabaseFixture : IAsyncLifetime
{
    private readonly IContainer _dbContainer;

    public DatabaseFixture()
    {
        _dbContainer = new ContainerBuilder()
            .WithImage("postgres:18.1")
            .WithEnvironment("POSTGRES_DB", "testsdelivery_notifications_test")
            .WithEnvironment("POSTGRES_USER", "postgres")
            .WithEnvironment("POSTGRES_PASSWORD", "postgres")
            .WithPortBinding(5437, 5432)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432))
            .Build();
    }

    public string ConnectionString => $"Host=localhost;Port=5437;Database=testsdelivery_notifications_test;Username=postgres;Password=postgres";

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
    }

    public async Task<NotificationDbContext> CreateDbContextAsync()
    {
        var options = new DbContextOptionsBuilder<NotificationDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        var context = new NotificationDbContext(options);
        await context.Database.MigrateAsync();
        return context;
    }
}
