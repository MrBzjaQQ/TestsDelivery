namespace NotificationService.Application.Specifications.TemplateSpecs;

public class TemplateExistsSpecification
{
    private readonly string _templatesBasePath;
    private readonly string _defaultLanguage;

    public TemplateExistsSpecification(string templatesBasePath, string defaultLanguage)
    {
        _templatesBasePath = templatesBasePath;
        _defaultLanguage = defaultLanguage;
    }

    public bool IsSatisfiedBy(string templateName)
    {
        var templatePath = Path.Combine(_templatesBasePath, _defaultLanguage, $"{templateName}.hbs");
        return File.Exists(templatePath);
    }
}
