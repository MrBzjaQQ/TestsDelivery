namespace NotificationService.Application.Contracts;

public interface ITemplateService
{
    Task<string> RenderAsync(string templateName, object templateData, CancellationToken ct);

    Task<bool> TemplateExistsAsync(string templateName, CancellationToken ct);
}
