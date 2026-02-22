namespace BffPortalService.Application.Contracts;

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key, CancellationToken ct);

    Task SetAsync<T>(string key, T value, TimeSpan expiration, CancellationToken ct);

    Task<T> GetOrAddAsync<T>(string key, Func<CancellationToken, Task<T>> factory, TimeSpan expiration, CancellationToken ct);

    Task RemoveAsync(string key, CancellationToken ct);

    Task RemoveByPrefixAsync(string prefix, CancellationToken ct);
}
