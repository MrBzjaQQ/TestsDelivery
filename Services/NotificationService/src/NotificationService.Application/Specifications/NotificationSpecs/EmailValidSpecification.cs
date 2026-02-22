using NotificationService.Domain.ValueObjects;

namespace NotificationService.Application.Specifications.NotificationSpecs;

public class EmailValidSpecification
{
    public bool IsSatisfiedBy(string email)
    {
        return EmailAddress.IsValidEmail(email);
    }
}
