using NotificationService.Application.Infrastructure.Database.Contract;
using NotificationService.Domain.Entities;
using NotificationService.Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace NotificationService.Infrastructure.Database.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly INotificationDbContext _context;

    public NotificationRepository(INotificationDbContext context)
    {
        _context = context;
    }

    public async Task<Notification?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await ((NotificationDbContext)_context).Notifications
            .FirstOrDefaultAsync(n => n.Id == id, ct);
    }

    public async Task<List<Notification>> GetPendingNotificationsAsync(CancellationToken ct)
    {
        return await ((NotificationDbContext)_context).Notifications
            .Where(n => n.Status == Domain.ValueObjects.NotificationStatus.Pending ||
                        n.Status == Domain.ValueObjects.NotificationStatus.Retrying)
            .OrderBy(n => n.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task AddAsync(Notification notification, CancellationToken ct)
    {
        await ((NotificationDbContext)_context).Notifications.AddAsync(notification, ct);
    }

    public async Task UpdateAsync(Notification notification, CancellationToken ct)
    {
        ((NotificationDbContext)_context).Notifications.Update(notification);
        await Task.CompletedTask;
    }
}
