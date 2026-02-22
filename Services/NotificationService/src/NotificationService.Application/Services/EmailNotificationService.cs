using NotificationService.Application.Contracts;
using NotificationService.Application.DTOs.Requests;
using NotificationService.Application.DTOs.Responses;
using NotificationService.Application.Exceptions;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Exceptions;
using NotificationService.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace NotificationService.Application.Services;

public class EmailNotificationService : IEmailNotificationService
{
    private readonly IEmailSender _emailSender;
    private readonly ITemplateService _templateService;
    private readonly INotificationQueueService _queueService;
    private readonly ILogger<EmailNotificationService> _logger;

    public EmailNotificationService(
        IEmailSender emailSender,
        ITemplateService templateService,
        INotificationQueueService queueService,
        ILogger<EmailNotificationService> logger)
    {
        _emailSender = emailSender;
        _templateService = templateService;
        _queueService = queueService;
        _logger = logger;
    }

    public async Task<SendEmailResponse> SendEmailAsync(SendEmailRequest request, CancellationToken ct)
    {
        if (!EmailAddress.IsValidEmail(request.To))
        {
            throw new ArgumentException($"Invalid email address: {request.To}");
        }

        var templateExists = await _templateService.TemplateExistsAsync(request.TemplateName, ct);
        if (!templateExists)
        {
            throw new TemplateNotFoundException(request.TemplateName);
        }

        string htmlBody;
        try
        {
            htmlBody = await _templateService.RenderAsync(request.TemplateName, request.TemplateData ?? new { }, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to render template {TemplateName}", request.TemplateName);
            throw new TemplateRenderException(request.TemplateName, ex);
        }

        var emailId = Guid.NewGuid();
        var notification = new Notification
        {
            Id = emailId,
            To = request.To,
            Subject = request.Subject,
            TemplateName = request.TemplateName,
            TemplateData = System.Text.Json.JsonSerializer.Serialize(request.TemplateData),
            Type = NotificationType.Email,
            Status = NotificationStatus.Pending,
            HtmlBody = htmlBody,
            CreatedAt = DateTime.UtcNow
        };

        await _queueService.EnqueueNotificationAsync(notification, ct);

        var emailMessage = new EmailMessage
        {
            Id = emailId,
            To = request.To,
            From = "noreply@testsdelivery.com",
            Subject = request.Subject,
            HtmlBody = htmlBody,
            IsHtml = true,
            CreatedAt = DateTime.UtcNow
        };

        try
        {
            await _emailSender.SendAsync(emailMessage, ct);
            await _queueService.MarkAsSentAsync(emailId, ct);

            _logger.LogInformation("Email sent to {To} with subject {Subject}", request.To, request.Subject);

            return new SendEmailResponse(emailId, request.To, request.Subject, DateTime.UtcNow, "Sent");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {To}", request.To);
            await _queueService.MarkAsFailedAsync(emailId, ex.Message, ct);
            throw new EmailSendFailedException(request.To, ex);
        }
    }

    public async Task<SendEmailResponse> SendEmailDirectAsync(EmailMessage message, CancellationToken ct)
    {
        if (!EmailAddress.IsValidEmail(message.To))
        {
            throw new ArgumentException($"Invalid email address: {message.To}");
        }

        try
        {
            await _emailSender.SendAsync(message, ct);

            _logger.LogInformation("Direct email sent to {To} with subject {Subject}", message.To, message.Subject);

            return new SendEmailResponse(message.Id, message.To, message.Subject, DateTime.UtcNow, "Sent");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send direct email to {To}", message.To);
            throw new EmailSendFailedException(message.To, ex);
        }
    }
}
