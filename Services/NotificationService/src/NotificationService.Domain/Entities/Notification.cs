using NotificationService.Domain.ValueObjects;

namespace NotificationService.Domain.Entities;

public class Notification
{
    public Guid Id { get; init; }

    public string To { get; init; } = string.Empty;

    public string Subject { get; init; } = string.Empty;

    public string TemplateName { get; init; } = string.Empty;

    public string? TemplateData { get; init; }

    public NotificationType Type { get; init; }

    public NotificationStatus Status { get; set; }

    public int RetryCount { get; set; }

    public int MaxRetries { get; init; } = 3;

    public string? ErrorMessage { get; set; }

    public string? HtmlBody { get; set; }

    public DateTime CreatedAt { get; init; }

    public DateTime? SentAt { get; set; }

    public Guid? RelatedEntityId { get; init; }

    public string? RelatedEntityType { get; init; }
}
