namespace NotificationService.Application.DTOs.Responses;

public record TestCreatedNotificationResponse(
    Guid NotificationId,
    Guid StudentId,
    Guid TestId,
    DateTime SentAt);
