using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Database.Context;

public interface IIdentityDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
