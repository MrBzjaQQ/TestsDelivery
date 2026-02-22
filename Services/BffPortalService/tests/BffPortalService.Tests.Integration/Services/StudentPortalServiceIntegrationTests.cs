using BffPortalService.Application.Services;
using BffPortalService.Application.Contracts;
using BffPortalService.Application.DTOs.Responses;
using BffPortalService.Tests.Integration.TestInfrastructure;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace BffPortalService.Tests.Integration.Services;

[Collection("Database")]
public class StudentPortalServiceIntegrationTests
{
    private readonly DatabaseFixture _fixture;

    public StudentPortalServiceIntegrationTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    private ServiceProvider CreateServiceProvider()
    {
        var services = new ServiceCollection();

        services.AddMemoryCache();
        services.AddScoped<ICacheService, CacheService>();
        services.AddScoped<IStudentPortalService, StudentPortalService>();
        services.AddSingleton(Mock.Of<IHttpClientFactory>());
        services.AddSingleton(Mock.Of<ILogger<StudentPortalService>>());
        services.AddSingleton(Mock.Of<ILogger<CacheService>>());

        return services.BuildServiceProvider();
    }

    [Fact]
    public async Task CacheService_Integration_ShouldCacheAndRetrieveValue()
    {
        var services = new ServiceCollection();
        services.AddMemoryCache();
        services.AddScoped<ICacheService, CacheService>();
        services.AddSingleton(Mock.Of<ILogger<CacheService>>());

        var serviceProvider = services.BuildServiceProvider();

        using var scope = serviceProvider.CreateScope();
        var cacheService = scope.ServiceProvider.GetRequiredService<ICacheService>();

        var key = "test-integration-key";
        var value = new StudentProfileDto
        {
            StudentId = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe"
        };

        await cacheService.SetAsync(key, value, TimeSpan.FromMinutes(5), CancellationToken.None);

        var retrieved = await cacheService.GetAsync<StudentProfileDto>(key, CancellationToken.None);

        retrieved.Should().NotBeNull();
        retrieved!.FirstName.Should().Be("John");
    }

    [Fact]
    public async Task CacheService_Integration_GetOrAddShouldCacheResult()
    {
        var services = new ServiceCollection();
        services.AddMemoryCache();
        services.AddScoped<ICacheService, CacheService>();
        services.AddSingleton(Mock.Of<ILogger<CacheService>>());

        var serviceProvider = services.BuildServiceProvider();

        using var scope = serviceProvider.CreateScope();
        var cacheService = scope.ServiceProvider.GetRequiredService<ICacheService>();

        var key = "test-getoradd-key";
        var factoryCallCount = 0;

        var result1 = await cacheService.GetOrAddAsync(
            key,
            _ =>
            {
                factoryCallCount++;
                return Task.FromResult(new StudentProfileDto { FirstName = "John" });
            },
            TimeSpan.FromMinutes(5),
            CancellationToken.None);

        var result2 = await cacheService.GetOrAddAsync(
            key,
            _ =>
            {
                factoryCallCount++;
                return Task.FromResult(new StudentProfileDto { FirstName = "Jane" });
            },
            TimeSpan.FromMinutes(5),
            CancellationToken.None);

        factoryCallCount.Should().Be(1);
        result1.FirstName.Should().Be("John");
        result2.FirstName.Should().Be("John");
    }

    [Fact]
    public async Task CacheService_Integration_RemoveByPrefixShouldWork()
    {
        var services = new ServiceCollection();
        services.AddMemoryCache();
        services.AddScoped<ICacheService, CacheService>();
        services.AddSingleton(Mock.Of<ILogger<CacheService>>());

        var serviceProvider = services.BuildServiceProvider();

        using var scope = serviceProvider.CreateScope();
        var cacheService = scope.ServiceProvider.GetRequiredService<ICacheService>();

        await cacheService.SetAsync("student:1", "value1", TimeSpan.FromMinutes(5), CancellationToken.None);
        await cacheService.SetAsync("student:2", "value2", TimeSpan.FromMinutes(5), CancellationToken.None);
        await cacheService.SetAsync("test:1", "value3", TimeSpan.FromMinutes(5), CancellationToken.None);

        await cacheService.RemoveByPrefixAsync("student", CancellationToken.None);

        var student1 = await cacheService.GetAsync<string>("student:1", CancellationToken.None);
        var student2 = await cacheService.GetAsync<string>("student:2", CancellationToken.None);
        var test1 = await cacheService.GetAsync<string>("test:1", CancellationToken.None);

        student1.Should().BeNull();
        student2.Should().BeNull();
        test1.Should().Be("value3");
    }
}
