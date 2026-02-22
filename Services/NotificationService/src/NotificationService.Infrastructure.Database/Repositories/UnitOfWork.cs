using NotificationService.Application.Infrastructure.Database.Contract;
using NotificationService.Infrastructure.Database.Context;

namespace NotificationService.Infrastructure.Database.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly INotificationDbContext _context;

    public UnitOfWork(INotificationDbContext context)
    {
        _context = context;
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct)
    {
        return await _context.SaveChangesAsync(ct);
    }
}
