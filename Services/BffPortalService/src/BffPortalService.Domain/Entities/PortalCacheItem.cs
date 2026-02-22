namespace BffPortalService.Domain.Entities;

public class PortalCacheItem
{
    public Guid Id { get; init; }

    public string CacheKey { get; init; } = string.Empty;

    public string CacheValue { get; init; } = string.Empty;

    public string CacheType { get; init; } = string.Empty;

    public DateTime CreatedAt { get; init; }

    public DateTime ExpiresAt { get; init; }
}
