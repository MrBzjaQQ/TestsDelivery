namespace NotificationService.Application.Exceptions;

public class TemplateNotFoundException : Exception
{
    public string TemplateName { get; }

    public TemplateNotFoundException(string templateName)
        : base($"Template '{templateName}' not found")
    {
        TemplateName = templateName;
    }
}
