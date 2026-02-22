using NotificationService.Domain.Entities;

namespace NotificationService.Application.Infrastructure.Database.Contract;

public interface INotificationRepository
{
    Task<Notification?> GetByIdAsync(Guid id, CancellationToken ct);

    Task<List<Notification>> GetPendingNotificationsAsync(CancellationToken ct);

    Task AddAsync(Notification notification, CancellationToken ct);

    Task UpdateAsync(Notification notification, CancellationToken ct);
}
