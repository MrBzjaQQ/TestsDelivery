using System.Collections.Concurrent;
using BffPortalService.Application.Contracts;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace BffPortalService.Application.Services;

public class CacheService : ICacheService
{
    private readonly IMemoryCache _cache;
    private readonly ILogger<CacheService> _logger;
    private readonly ConcurrentDictionary<string, byte> _keys = new();

    public CacheService(IMemoryCache cache, ILogger<CacheService> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public Task<T?> GetAsync<T>(string key, CancellationToken ct)
    {
        if (_cache.TryGetValue(key, out T? cached))
        {
            _logger.LogDebug("Cache hit for key {Key}", key);
            return Task.FromResult(cached);
        }

        _logger.LogDebug("Cache miss for key {Key}", key);
        return Task.FromResult<T?>(default);
    }

    public Task SetAsync<T>(string key, T value, TimeSpan expiration, CancellationToken ct)
    {
        var options = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration,
            SlidingExpiration = TimeSpan.FromMinutes(Math.Min(expiration.TotalMinutes / 2, 5))
        };

        options.RegisterPostEvictionCallback((evictedKey, _, _, _) =>
        {
            if (evictedKey is string keyString)
            {
                _keys.TryRemove(keyString, out _);
            }
        });

        _cache.Set(key, value, options);
        _keys.TryAdd(key, 0);
        _logger.LogDebug("Cache set for key {Key} with expiration {Expiration}", key, expiration);

        return Task.CompletedTask;
    }

    public async Task<T> GetOrAddAsync<T>(string key, Func<CancellationToken, Task<T>> factory, TimeSpan expiration, CancellationToken ct)
    {
        if (_cache.TryGetValue(key, out T? cached) && cached is not null)
        {
            _logger.LogDebug("Cache hit for key {Key}", key);
            return cached;
        }

        _logger.LogDebug("Cache miss for key {Key}, fetching data", key);

        var result = await factory(ct);

        if (result is not null)
        {
            await SetAsync(key, result, expiration, ct);
        }

        return result;
    }

    public Task RemoveAsync(string key, CancellationToken ct)
    {
        _cache.Remove(key);
        _keys.TryRemove(key, out _);
        _logger.LogDebug("Cache removed for key {Key}", key);
        return Task.CompletedTask;
    }

    public Task RemoveByPrefixAsync(string prefix, CancellationToken ct)
    {
        var keysToRemove = _keys.Keys
            .Where(k => k.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            .ToList();

        foreach (var key in keysToRemove)
        {
            _cache.Remove(key);
            _keys.TryRemove(key, out _);
            _logger.LogDebug("Cache removed for key {Key}", key);
        }

        return Task.CompletedTask;
    }
}
