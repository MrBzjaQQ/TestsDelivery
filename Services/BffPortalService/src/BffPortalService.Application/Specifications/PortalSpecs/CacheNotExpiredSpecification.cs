namespace BffPortalService.Application.Specifications.PortalSpecs;

public class CacheNotExpiredSpecification
{
    public DateTime CreatedAt { get; }

    public TimeSpan CacheDuration { get; }

    public CacheNotExpiredSpecification(DateTime createdAt, TimeSpan cacheDuration)
    {
        CreatedAt = createdAt;
        CacheDuration = cacheDuration;
    }

    public bool IsSatisfiedBy()
    {
        var expirationTime = CreatedAt.Add(CacheDuration);
        return DateTime.UtcNow < expirationTime;
    }
}
