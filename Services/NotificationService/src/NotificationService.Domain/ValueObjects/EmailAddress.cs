using System.Net.Mail;

namespace NotificationService.Domain.ValueObjects;

public record EmailAddress
{
    public string Value { get; }

    public EmailAddress(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Email address cannot be empty", nameof(value));
        }

        if (!IsValidEmail(value))
        {
            throw new ArgumentException($"Invalid email address: {value}", nameof(value));
        }

        Value = value;
    }

    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return false;
        }

        try
        {
            var addr = new MailAddress(email);
            return addr.Host.Contains(".") && addr.Host.Length > 2;
        }
        catch
        {
            return false;
        }
    }

    public static implicit operator string(EmailAddress emailAddress) => emailAddress.Value;

    public override string ToString() => Value;
}
