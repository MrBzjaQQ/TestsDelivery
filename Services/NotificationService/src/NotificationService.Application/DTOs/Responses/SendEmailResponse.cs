namespace NotificationService.Application.DTOs.Responses;

public record SendEmailResponse(
    Guid EmailId,
    string To,
    string Subject,
    DateTime SentAt,
    string Status);
