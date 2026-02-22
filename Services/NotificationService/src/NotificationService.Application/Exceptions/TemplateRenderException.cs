namespace NotificationService.Application.Exceptions;

public class TemplateRenderException : Exception
{
    public string TemplateName { get; }

    public TemplateRenderException(string templateName, Exception? innerException = null)
        : base($"Failed to render template '{templateName}'", innerException)
    {
        TemplateName = templateName;
    }
}
