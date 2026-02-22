using NotificationService.Application.DTOs.Requests;
using NotificationService.Application.DTOs.Responses;
using NotificationService.Domain.Entities;

namespace NotificationService.Application.Contracts;

public interface IEmailNotificationService
{
    Task<SendEmailResponse> SendEmailAsync(SendEmailRequest request, CancellationToken ct);

    Task<SendEmailResponse> SendEmailDirectAsync(EmailMessage message, CancellationToken ct);
}
