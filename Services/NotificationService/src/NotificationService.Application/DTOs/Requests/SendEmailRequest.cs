namespace NotificationService.Application.DTOs.Requests;

public class SendEmailRequest
{
    public string To { get; set; } = string.Empty;

    public string Subject { get; set; } = string.Empty;

    public string TemplateName { get; set; } = string.Empty;

    public object? TemplateData { get; set; }
}
