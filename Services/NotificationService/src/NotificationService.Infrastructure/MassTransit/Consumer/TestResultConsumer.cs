using MassTransit;
using NotificationService.Application.Contracts;
using NotificationService.Application.DTOs.Requests;
using NotificationService.Infrastructure.MassTransit.MessageTypes;
using Microsoft.Extensions.Logging;

namespace NotificationService.Infrastructure.MassTransit.Consumer;

public class TestResultConsumer : IConsumer<TestResultReceivedEvent>
{
    private readonly IEmailNotificationService _emailService;
    private readonly ILogger<TestResultConsumer> _logger;

    public TestResultConsumer(IEmailNotificationService emailService, ILogger<TestResultConsumer> logger)
    {
        _emailService = emailService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<TestResultReceivedEvent> context)
    {
        var @event = context.Message;

        _logger.LogInformation("Processing test result notification for {StudentEmail}", @event.StudentEmail);

        var templateData = new
        {
            StudentName = @event.StudentName,
            TestTitle = $"Test {@event.TestId}",
            Score = @event.Score,
            MaxScore = @event.MaxScore,
            Percentage = @event.Percentage,
            IsPassed = @event.IsPassed
        };

        var sendRequest = new SendEmailRequest
        {
            To = @event.StudentEmail,
            Subject = "Test Result Available",
            TemplateName = "test-result",
            TemplateData = templateData
        };

        try
        {
            await _emailService.SendEmailAsync(sendRequest, context.CancellationToken);
            _logger.LogInformation("Test result notification sent to {Email}", @event.StudentEmail);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send test result notification to {Email}", @event.StudentEmail);
            throw;
        }
    }
}
