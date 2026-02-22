namespace IdentityService.Domain.Exceptions;

public class EmailNotVerifiedException : Exception
{
    public EmailNotVerifiedException()
        : base("Please verify your email address first")
    {
    }

    public EmailNotVerifiedException(string email)
        : base($"Email '{email}' is not verified. Please verify your email address first.")
    {
        Email = email;
    }

    public string? Email { get; }
}
