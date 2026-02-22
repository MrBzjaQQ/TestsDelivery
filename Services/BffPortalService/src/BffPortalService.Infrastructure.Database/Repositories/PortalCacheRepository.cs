using BffPortalService.Domain.Entities;
using BffPortalService.Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace BffPortalService.Infrastructure.Database.Repositories;

public class PortalCacheRepository
{
    private readonly BffPortalDbContext _context;

    public PortalCacheRepository(BffPortalDbContext context)
    {
        _context = context;
    }

    public async Task<PortalCacheItem?> GetByKeyAsync(string cacheKey, CancellationToken ct)
    {
        return await _context.PortalCacheItems
            .FirstOrDefaultAsync(x => x.CacheKey == cacheKey, ct);
    }

    public async Task<IEnumerable<PortalCacheItem>> GetByPrefixAsync(string prefix, CancellationToken ct)
    {
        return await _context.PortalCacheItems
            .Where(x => x.CacheKey.StartsWith(prefix))
            .ToListAsync(ct);
    }

    public async Task AddAsync(PortalCacheItem item, CancellationToken ct)
    {
        await _context.PortalCacheItems.AddAsync(item, ct);
    }

    public void Delete(PortalCacheItem item)
    {
        _context.PortalCacheItems.Remove(item);
    }

    public async Task DeleteByPrefixAsync(string prefix, CancellationToken ct)
    {
        var items = await GetByPrefixAsync(prefix, ct);
        _context.PortalCacheItems.RemoveRange(items);
    }

    public async Task CleanupExpiredAsync(CancellationToken ct)
    {
        var expiredItems = await _context.PortalCacheItems
            .Where(x => x.ExpiresAt < DateTime.UtcNow)
            .ToListAsync(ct);

        _context.PortalCacheItems.RemoveRange(expiredItems);
    }
}
