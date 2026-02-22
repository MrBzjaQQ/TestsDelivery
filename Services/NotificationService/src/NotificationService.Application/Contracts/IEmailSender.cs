using NotificationService.Domain.Entities;

namespace NotificationService.Application.Contracts;

public interface IEmailSender
{
    Task SendAsync(EmailMessage message, CancellationToken ct);
}
