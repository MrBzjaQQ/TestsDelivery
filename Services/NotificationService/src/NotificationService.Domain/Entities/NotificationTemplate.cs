namespace NotificationService.Domain.Entities;

public class NotificationTemplate
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string Subject { get; init; } = string.Empty;

    public string Content { get; init; } = string.Empty;

    public string Language { get; init; } = "en";

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; init; }

    public DateTime? UpdatedAt { get; set; }
}
