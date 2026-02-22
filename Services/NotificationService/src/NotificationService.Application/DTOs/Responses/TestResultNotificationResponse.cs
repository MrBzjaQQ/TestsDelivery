namespace NotificationService.Application.DTOs.Responses;

public record TestResultNotificationResponse(
    Guid StudentId,
    Guid TestId,
    int Score,
    int MaxScore,
    bool IsPassed,
    DateTime SentAt);
