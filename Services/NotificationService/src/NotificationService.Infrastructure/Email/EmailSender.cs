using NotificationService.Application.Contracts;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace NotificationService.Infrastructure.Email;

public class EmailSender : IEmailSender
{
    private readonly SmtpSettings _settings;
    private readonly ILogger<EmailSender> _logger;

    public EmailSender(SmtpSettings settings, ILogger<EmailSender> logger)
    {
        _settings = settings;
        _logger = logger;
    }

    public async Task SendAsync(EmailMessage message, CancellationToken ct)
    {
        var mimeMessage = new MimeMessage();
        mimeMessage.From.Add(new MailboxAddress(_settings.DisplayName, _settings.From));
        mimeMessage.To.Add(new MailboxAddress(string.Empty, message.To));
        mimeMessage.Subject = message.Subject;

        var bodyBuilder = new BodyBuilder();
        if (message.IsHtml && !string.IsNullOrEmpty(message.HtmlBody))
        {
            bodyBuilder.HtmlBody = message.HtmlBody;
        }
        else if (!string.IsNullOrEmpty(message.PlainTextBody))
        {
            bodyBuilder.TextBody = message.PlainTextBody;
        }

        mimeMessage.Body = bodyBuilder.ToMessageBody();

        try
        {
            using var client = new MailKit.Net.Smtp.SmtpClient();

            await client.ConnectAsync(_settings.Host, _settings.Port, _settings.EnableSsl, ct);

            if (!string.IsNullOrEmpty(_settings.Username) && !string.IsNullOrEmpty(_settings.Password))
            {
                await client.AuthenticateAsync(_settings.Username, _settings.Password, ct);
            }

            await client.SendAsync(mimeMessage, ct);
            await client.DisconnectAsync(true, ct);

            _logger.LogInformation("Email sent successfully to {To}", message.To);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {To}", message.To);
            throw new EmailSendFailedException(message.To, ex);
        }
    }
}
