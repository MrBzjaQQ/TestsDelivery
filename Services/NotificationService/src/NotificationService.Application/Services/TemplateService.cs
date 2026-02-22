using NotificationService.Application.Contracts;
using NotificationService.Application.Exceptions;
using Microsoft.Extensions.Logging;

namespace NotificationService.Application.Services;

public class TemplateService : ITemplateService
{
    private readonly string _templatesBasePath;
    private readonly string _defaultLanguage;
    private readonly ILogger<TemplateService> _logger;

    public TemplateService(string templatesBasePath, string defaultLanguage, ILogger<TemplateService> logger)
    {
        _templatesBasePath = templatesBasePath;
        _defaultLanguage = defaultLanguage;
        _logger = logger;
    }

    public async Task<string> RenderAsync(string templateName, object templateData, CancellationToken ct)
    {
        var templatePath = GetTemplatePath(templateName);

        if (!File.Exists(templatePath))
        {
            _logger.LogWarning("Template {TemplateName} not found at {TemplatePath}", templateName, templatePath);
            throw new TemplateNotFoundException(templateName);
        }

        var templateContent = await File.ReadAllTextAsync(templatePath, ct);

        try
        {
            var handlebars = HandlebarsDotNet.Handlebars.Create();
            var compiledTemplate = handlebars.Compile(templateContent);
            var result = compiledTemplate(templateData);

            _logger.LogDebug("Template {TemplateName} rendered successfully", templateName);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to render template {TemplateName}", templateName);
            throw new TemplateRenderException(templateName, ex);
        }
    }

    public Task<bool> TemplateExistsAsync(string templateName, CancellationToken ct)
    {
        var templatePath = GetTemplatePath(templateName);
        return Task.FromResult(File.Exists(templatePath));
    }

    private string GetTemplatePath(string templateName)
    {
        return Path.Combine(_templatesBasePath, _defaultLanguage, $"{templateName}.hbs");
    }
}
