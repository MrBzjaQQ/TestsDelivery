namespace NotificationService.Domain.Exceptions;

public class EmailSendFailedException : Exception
{
    public string EmailAddress { get; }

    public EmailSendFailedException(string emailAddress, Exception? innerException = null)
        : base($"Failed to send email to {emailAddress}", innerException)
    {
        EmailAddress = emailAddress;
    }
}
