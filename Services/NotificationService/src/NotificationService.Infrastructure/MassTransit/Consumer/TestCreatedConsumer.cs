using MassTransit;
using NotificationService.Application.Contracts;
using NotificationService.Application.DTOs.Requests;
using NotificationService.Infrastructure.MassTransit.MessageTypes;
using Microsoft.Extensions.Logging;

namespace NotificationService.Infrastructure.MassTransit.Consumer;

public class TestCreatedConsumer : IConsumer<TestCreatedEvent>
{
    private readonly IEmailNotificationService _emailService;
    private readonly ILogger<TestCreatedConsumer> _logger;

    public TestCreatedConsumer(IEmailNotificationService emailService, ILogger<TestCreatedConsumer> logger)
    {
        _emailService = emailService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<TestCreatedEvent> context)
    {
        var @event = context.Message;

        _logger.LogInformation("Processing test created notification for {TestTitle}", @event.Title);

        var templateData = new
        {
            TestTitle = @event.Title,
            StudentName = @event.StudentName,
            TestLink = $"https://portal.com/tests/{@event.TestId}"
        };

        var sendRequest = new SendEmailRequest
        {
            To = @event.StudentEmail,
            Subject = $"New Test Available: {@event.Title}",
            TemplateName = "test-created",
            TemplateData = templateData
        };

        try
        {
            await _emailService.SendEmailAsync(sendRequest, context.CancellationToken);
            _logger.LogInformation("Test created notification sent to {Email}", @event.StudentEmail);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send test created notification to {Email}", @event.StudentEmail);
            throw;
        }
    }
}
