namespace NotificationService.Domain.Entities;

public class EmailMessage
{
    public Guid Id { get; init; }

    public string To { get; init; } = string.Empty;

    public string From { get; init; } = string.Empty;

    public string Subject { get; init; } = string.Empty;

    public string? HtmlBody { get; init; }

    public string? PlainTextBody { get; init; }

    public bool IsHtml { get; init; }

    public DateTime CreatedAt { get; init; }
}
