using NotificationService.Application.Contracts;
using NotificationService.Application.DTOs.Responses;
using NotificationService.Application.Infrastructure.Database.Contract;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Exceptions;
using NotificationService.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace NotificationService.Application.Services;

public class NotificationQueueService : INotificationQueueService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<NotificationQueueService> _logger;

    public NotificationQueueService(
        INotificationRepository notificationRepository,
        IUnitOfWork unitOfWork,
        ILogger<NotificationQueueService> logger)
    {
        _notificationRepository = notificationRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Notification> EnqueueNotificationAsync(Notification notification, CancellationToken ct)
    {
        notification.Status = NotificationStatus.Pending;

        await _notificationRepository.AddAsync(notification, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        _logger.LogInformation("Notification {NotificationId} enqueued for {To}", notification.Id, notification.To);

        return notification;
    }

    public async Task<Notification?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _notificationRepository.GetByIdAsync(id, ct);
    }

    public async Task<PendingNotificationsResponse> GetPendingNotificationsAsync(CancellationToken ct)
    {
        var notifications = await _notificationRepository.GetPendingNotificationsAsync(ct);

        var dtos = notifications.Select(n => new NotificationStatusDto(
            n.Id,
            n.To,
            n.Subject,
            n.TemplateName,
            n.Status,
            n.RetryCount,
            n.CreatedAt,
            n.SentAt,
            n.ErrorMessage)).ToList();

        return new PendingNotificationsResponse(dtos, dtos.Count);
    }

    public async Task MarkAsSentAsync(Guid notificationId, CancellationToken ct)
    {
        var notification = await _notificationRepository.GetByIdAsync(notificationId, ct);
        if (notification == null)
        {
            throw new NotificationNotFoundException(notificationId);
        }

        notification.Status = NotificationStatus.Sent;
        notification.SentAt = DateTime.UtcNow;

        await _notificationRepository.UpdateAsync(notification, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        _logger.LogInformation("Notification {NotificationId} marked as sent", notificationId);
    }

    public async Task MarkAsFailedAsync(Guid notificationId, string errorMessage, CancellationToken ct)
    {
        var notification = await _notificationRepository.GetByIdAsync(notificationId, ct);
        if (notification == null)
        {
            throw new NotificationNotFoundException(notificationId);
        }

        notification.Status = notification.RetryCount >= notification.MaxRetries
            ? NotificationStatus.Failed
            : NotificationStatus.Retrying;

        notification.ErrorMessage = errorMessage;

        await _notificationRepository.UpdateAsync(notification, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        _logger.LogWarning("Notification {NotificationId} marked as failed: {ErrorMessage}", notificationId, errorMessage);
    }

    public async Task IncrementRetryCountAsync(Guid notificationId, CancellationToken ct)
    {
        var notification = await _notificationRepository.GetByIdAsync(notificationId, ct);
        if (notification == null)
        {
            throw new NotificationNotFoundException(notificationId);
        }

        notification.RetryCount++;

        await _notificationRepository.UpdateAsync(notification, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }
}
