using BffPortalService.Infrastructure.Database;
using BffPortalService.Infrastructure.Database.Context;
using BffPortalService.Infrastructure.Database.Repositories;
using BffPortalService.Tests.Integration.TestInfrastructure;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BffPortalService.Tests.Integration.Services;

[Collection("Database")]
public class DatabaseIntegrationTests : IAsyncLifetime
{
    private readonly DatabaseFixture _fixture;
    private ServiceProvider _serviceProvider = null!;

    public DatabaseIntegrationTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    public Task InitializeAsync()
    {
        var services = new ServiceCollection();
        services.AddDatabase(_fixture.ConnectionString);
        _serviceProvider = services.BuildServiceProvider();

        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BffPortalDbContext>();
        context.Database.Migrate();

        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        await _serviceProvider.DisposeAsync();
    }

    [Fact]
    public async Task Database_CanConnectAndQuery()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BffPortalDbContext>();

        var canConnect = await context.Database.CanConnectAsync();

        canConnect.Should().BeTrue();
    }

    [Fact]
    public async Task PortalCacheRepository_CanAddAndRetrieveItem()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BffPortalDbContext>();
        var repository = new PortalCacheRepository(context);

        var cacheItem = new Domain.Entities.PortalCacheItem
        {
            Id = Guid.NewGuid(),
            CacheKey = "test-key",
            CacheValue = "test-value",
            CacheType = "Test",
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddHours(1)
        };

        await repository.AddAsync(cacheItem, CancellationToken.None);
        await context.SaveChangesAsync(CancellationToken.None);

        var retrieved = await repository.GetByKeyAsync("test-key", CancellationToken.None);

        retrieved.Should().NotBeNull();
        retrieved!.CacheValue.Should().Be("test-value");
    }

    [Fact]
    public async Task PortalCacheRepository_CanDeleteByPrefix()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BffPortalDbContext>();
        var repository = new PortalCacheRepository(context);

        var items = new[]
        {
            new Domain.Entities.PortalCacheItem
            {
                Id = Guid.NewGuid(),
                CacheKey = "prefix-key1",
                CacheValue = "value1",
                CacheType = "Test",
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            },
            new Domain.Entities.PortalCacheItem
            {
                Id = Guid.NewGuid(),
                CacheKey = "prefix-key2",
                CacheValue = "value2",
                CacheType = "Test",
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            },
            new Domain.Entities.PortalCacheItem
            {
                Id = Guid.NewGuid(),
                CacheKey = "other-key",
                CacheValue = "value3",
                CacheType = "Test",
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            }
        };

        foreach (var item in items)
        {
            await repository.AddAsync(item, CancellationToken.None);
        }

        await context.SaveChangesAsync(CancellationToken.None);

        await repository.DeleteByPrefixAsync("prefix", CancellationToken.None);
        await context.SaveChangesAsync(CancellationToken.None);

        var remaining = await context.PortalCacheItems.ToListAsync();

        remaining.Should().HaveCount(1);
        remaining[0].CacheKey.Should().Be("other-key");
    }

    [Fact]
    public async Task PortalCacheRepository_CanCleanupExpired()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BffPortalDbContext>();
        var repository = new PortalCacheRepository(context);

        var items = new[]
        {
            new Domain.Entities.PortalCacheItem
            {
                Id = Guid.NewGuid(),
                CacheKey = "expired-key",
                CacheValue = "value1",
                CacheType = "Test",
                CreatedAt = DateTime.UtcNow.AddHours(-2),
                ExpiresAt = DateTime.UtcNow.AddHours(-1)
            },
            new Domain.Entities.PortalCacheItem
            {
                Id = Guid.NewGuid(),
                CacheKey = "valid-key",
                CacheValue = "value2",
                CacheType = "Test",
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            }
        };

        foreach (var item in items)
        {
            await repository.AddAsync(item, CancellationToken.None);
        }

        await context.SaveChangesAsync(CancellationToken.None);

        await repository.CleanupExpiredAsync(CancellationToken.None);
        await context.SaveChangesAsync(CancellationToken.None);

        var remaining = await context.PortalCacheItems.ToListAsync();

        remaining.Should().HaveCount(1);
        remaining[0].CacheKey.Should().Be("valid-key");
    }
}
