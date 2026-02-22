using BffPortalService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BffPortalService.Infrastructure.Database.Context;

public interface IBffPortalDbContext
{
    DbSet<PortalCacheItem> PortalCacheItems { get; }

    DbSet<PortalSettings> PortalSettings { get; }

    Task<int> SaveChangesAsync(CancellationToken ct);
}
