using NotificationService.Application.DTOs.Responses;
using NotificationService.Domain.Entities;

namespace NotificationService.Application.Contracts;

public interface INotificationQueueService
{
    Task<Notification> EnqueueNotificationAsync(Notification notification, CancellationToken ct);

    Task<Notification?> GetByIdAsync(Guid id, CancellationToken ct);

    Task<PendingNotificationsResponse> GetPendingNotificationsAsync(CancellationToken ct);

    Task MarkAsSentAsync(Guid notificationId, CancellationToken ct);

    Task MarkAsFailedAsync(Guid notificationId, string errorMessage, CancellationToken ct);

    Task IncrementRetryCountAsync(Guid notificationId, CancellationToken ct);
}
