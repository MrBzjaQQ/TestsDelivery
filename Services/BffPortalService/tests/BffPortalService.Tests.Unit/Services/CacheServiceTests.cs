using BffPortalService.Application.Services;
using BffPortalService.Application.Contracts;
using BffPortalService.Domain.Exceptions;
using BffPortalService.Domain.ValueObjects;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace BffPortalService.Tests.Unit.Services;

public class CacheServiceTests
{
    private readonly Mock<ILogger<CacheService>> _loggerMock;
    private readonly IMemoryCache _memoryCache;
    private readonly CacheService _cacheService;

    public CacheServiceTests()
    {
        _loggerMock = new Mock<ILogger<CacheService>>();
        _memoryCache = new MemoryCache(new MemoryCacheOptions());
        _cacheService = new CacheService(_memoryCache, _loggerMock.Object);
    }

    [Fact]
    public async Task GetAsync_WhenKeyExists_ShouldReturnValue()
    {
        var key = "test-key";
        var value = "test-value";
        _memoryCache.Set(key, value);

        var result = await _cacheService.GetAsync<string>(key, CancellationToken.None);

        result.Should().Be(value);
    }

    [Fact]
    public async Task GetAsync_WhenKeyDoesNotExist_ShouldReturnDefault()
    {
        var key = "non-existent-key";

        var result = await _cacheService.GetAsync<string>(key, CancellationToken.None);

        result.Should().BeNull();
    }

    [Fact]
    public async Task SetAsync_ShouldStoreValueInCache()
    {
        var key = "test-key";
        var value = "test-value";
        var expiration = TimeSpan.FromMinutes(5);

        await _cacheService.SetAsync(key, value, expiration, CancellationToken.None);

        var cachedValue = _memoryCache.Get<string>(key);
        cachedValue.Should().Be(value);
    }

    [Fact]
    public async Task GetOrAddAsync_WhenKeyExists_ShouldReturnCachedValue()
    {
        var key = "test-key";
        var cachedValue = "cached-value";
        _memoryCache.Set(key, cachedValue);

        var factoryCalled = false;
        var result = await _cacheService.GetOrAddAsync(
            key,
            _ =>
            {
                factoryCalled = true;
                return Task.FromResult("new-value");
            },
            TimeSpan.FromMinutes(5),
            CancellationToken.None);

        result.Should().Be(cachedValue);
        factoryCalled.Should().BeFalse();
    }

    [Fact]
    public async Task GetOrAddAsync_WhenKeyDoesNotExist_ShouldCallFactoryAndCache()
    {
        var key = "test-key";
        var newValue = "new-value";

        var result = await _cacheService.GetOrAddAsync(
            key,
            _ => Task.FromResult(newValue),
            TimeSpan.FromMinutes(5),
            CancellationToken.None);

        result.Should().Be(newValue);
        _memoryCache.Get<string>(key).Should().Be(newValue);
    }

    [Fact]
    public async Task RemoveAsync_ShouldRemoveValueFromCache()
    {
        var key = "test-key";
        _memoryCache.Set(key, "value");

        await _cacheService.RemoveAsync(key, CancellationToken.None);

        _memoryCache.TryGetValue(key, out _).Should().BeFalse();
    }

    [Fact]
    public async Task RemoveByPrefixAsync_ShouldRemoveMatchingKeys()
    {
        await _cacheService.SetAsync("prefix-key1", "value1", TimeSpan.FromMinutes(5), CancellationToken.None);
        await _cacheService.SetAsync("prefix-key2", "value2", TimeSpan.FromMinutes(5), CancellationToken.None);
        await _cacheService.SetAsync("other-key", "value3", TimeSpan.FromMinutes(5), CancellationToken.None);

        await _cacheService.RemoveByPrefixAsync("prefix", CancellationToken.None);

        _memoryCache.TryGetValue("prefix-key1", out _).Should().BeFalse();
        _memoryCache.TryGetValue("prefix-key2", out _).Should().BeFalse();
        _memoryCache.TryGetValue("other-key", out _).Should().BeTrue();
    }
}
