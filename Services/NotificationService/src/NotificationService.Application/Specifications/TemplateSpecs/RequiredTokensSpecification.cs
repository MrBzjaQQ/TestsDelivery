namespace NotificationService.Application.Specifications.TemplateSpecs;

public class RequiredTokensSpecification
{
    private readonly string[] _requiredTokens;

    public RequiredTokensSpecification(string[] requiredTokens)
    {
        _requiredTokens = requiredTokens;
    }

    public bool IsSatisfiedBy(string templateContent)
    {
        foreach (var token in _requiredTokens)
        {
            if (!templateContent.Contains($"{{{{{token}}}}}"))
            {
                return false;
            }
        }

        return true;
    }
}
