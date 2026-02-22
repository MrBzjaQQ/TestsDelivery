using MassTransit;
using NotificationService.Application.Contracts;
using NotificationService.Application.DTOs.Requests;
using NotificationService.Infrastructure.MassTransit.MessageTypes;
using Microsoft.Extensions.Logging;

namespace NotificationService.Infrastructure.MassTransit.Consumer;

public class UserRegisteredConsumer : IConsumer<UserRegisteredEvent>
{
    private readonly IEmailNotificationService _emailService;
    private readonly ILogger<UserRegisteredConsumer> _logger;

    public UserRegisteredConsumer(IEmailNotificationService emailService, ILogger<UserRegisteredConsumer> logger)
    {
        _emailService = emailService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<UserRegisteredEvent> context)
    {
        var @event = context.Message;

        _logger.LogInformation("Processing user registered notification for {Email}", @event.Email);

        var templateData = new
        {
            FirstName = @event.FirstName,
            LastName = @event.LastName,
            Email = @event.Email,
            Role = @event.Role
        };

        var sendRequest = new SendEmailRequest
        {
            To = @event.Email,
            Subject = "Welcome to TestsDelivery!",
            TemplateName = "user-registered",
            TemplateData = templateData
        };

        try
        {
            await _emailService.SendEmailAsync(sendRequest, context.CancellationToken);
            _logger.LogInformation("User registered notification sent to {Email}", @event.Email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send user registered notification to {Email}", @event.Email);
            throw;
        }
    }
}
