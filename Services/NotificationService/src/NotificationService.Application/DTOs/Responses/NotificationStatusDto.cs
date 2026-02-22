using NotificationService.Domain.ValueObjects;

namespace NotificationService.Application.DTOs.Responses;

public record NotificationStatusDto(
    Guid Id,
    string To,
    string Subject,
    string TemplateName,
    NotificationStatus Status,
    int RetryCount,
    DateTime CreatedAt,
    DateTime? SentAt,
    string? ErrorMessage);
