using NotificationService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace NotificationService.Infrastructure.Database.Context;

public interface INotificationDbContext
{
    DbSet<Notification> Notifications { get; }

    Task<int> SaveChangesAsync(CancellationToken ct);
}
