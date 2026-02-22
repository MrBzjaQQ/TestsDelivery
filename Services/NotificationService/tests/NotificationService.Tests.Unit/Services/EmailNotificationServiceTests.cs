using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NotificationService.Application.Contracts;
using NotificationService.Application.DTOs.Requests;
using NotificationService.Application.DTOs.Responses;
using NotificationService.Application.Exceptions;
using NotificationService.Application.Services;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Exceptions;
using Xunit;

namespace NotificationService.Tests.Unit.Services;

public class EmailNotificationServiceTests
{
    private readonly Mock<IEmailSender> _mockEmailSender;
    private readonly Mock<ITemplateService> _mockTemplateService;
    private readonly Mock<INotificationQueueService> _mockQueueService;
    private readonly Mock<ILogger<EmailNotificationService>> _mockLogger;
    private readonly EmailNotificationService _service;

    public EmailNotificationServiceTests()
    {
        _mockEmailSender = new Mock<IEmailSender>();
        _mockTemplateService = new Mock<ITemplateService>();
        _mockQueueService = new Mock<INotificationQueueService>();
        _mockLogger = new Mock<ILogger<EmailNotificationService>>();
        _service = new EmailNotificationService(
            _mockEmailSender.Object,
            _mockTemplateService.Object,
            _mockQueueService.Object,
            _mockLogger.Object);
    }

    [Fact]
    public async Task SendEmailAsync_Should_CallEmailSender()
    {
        var request = new SendEmailRequest
        {
            To = "test@example.com",
            Subject = "Test Subject",
            TemplateName = "test-created",
            TemplateData = new { testTitle = "Math" }
        };

        _mockTemplateService.Setup(s => s.TemplateExistsAsync("test-created", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _mockTemplateService.Setup(s => s.RenderAsync("test-created", It.IsAny<object>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("<html>Test Email</html>");

        _mockQueueService.Setup(s => s.EnqueueNotificationAsync(It.IsAny<Notification>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Notification n, CancellationToken _) => n);

        _mockQueueService.Setup(s => s.MarkAsSentAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mockEmailSender.Setup(s => s.SendAsync(It.IsAny<EmailMessage>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await _service.SendEmailAsync(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.To.Should().Be("test@example.com");
        result.Subject.Should().Be("Test Subject");
        result.Status.Should().Be("Sent");

        _mockEmailSender.Verify(s => s.SendAsync(It.IsAny<EmailMessage>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task SendEmailAsync_Should_ThrowArgumentException_When_EmailIsInvalid()
    {
        var request = new SendEmailRequest
        {
            To = "invalid-email",
            Subject = "Test Subject",
            TemplateName = "test-created"
        };

        var act = async () => await _service.SendEmailAsync(request, CancellationToken.None);

        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Invalid email address: invalid-email*");
    }

    [Fact]
    public async Task SendEmailAsync_Should_ThrowTemplateNotFoundException_When_TemplateDoesNotExist()
    {
        var request = new SendEmailRequest
        {
            To = "test@example.com",
            Subject = "Test Subject",
            TemplateName = "non-existent-template"
        };

        _mockTemplateService.Setup(s => s.TemplateExistsAsync("non-existent-template", It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var act = async () => await _service.SendEmailAsync(request, CancellationToken.None);

        await act.Should().ThrowAsync<TemplateNotFoundException>()
            .WithMessage("Template 'non-existent-template' not found");
    }

    [Fact]
    public async Task SendEmailAsync_Should_ThrowEmailSendFailedException_When_EmailSendFails()
    {
        var request = new SendEmailRequest
        {
            To = "test@example.com",
            Subject = "Test Subject",
            TemplateName = "test-created"
        };

        _mockTemplateService.Setup(s => s.TemplateExistsAsync("test-created", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _mockTemplateService.Setup(s => s.RenderAsync("test-created", It.IsAny<object>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("<html>Test Email</html>");

        _mockQueueService.Setup(s => s.EnqueueNotificationAsync(It.IsAny<Notification>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Notification n, CancellationToken _) => n);

        _mockQueueService.Setup(s => s.MarkAsFailedAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mockEmailSender.Setup(s => s.SendAsync(It.IsAny<EmailMessage>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("SMTP error"));

        var act = async () => await _service.SendEmailAsync(request, CancellationToken.None);

        await act.Should().ThrowAsync<EmailSendFailedException>()
            .WithMessage("Failed to send email to test@example.com");
    }

    [Fact]
    public async Task SendEmailDirectAsync_Should_CallEmailSender()
    {
        var message = new EmailMessage
        {
            Id = Guid.NewGuid(),
            To = "test@example.com",
            From = "noreply@testsdelivery.com",
            Subject = "Test Subject",
            HtmlBody = "<html>Test</html>",
            IsHtml = true,
            CreatedAt = DateTime.UtcNow
        };

        _mockEmailSender.Setup(s => s.SendAsync(message, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await _service.SendEmailDirectAsync(message, CancellationToken.None);

        result.Should().NotBeNull();
        result.To.Should().Be("test@example.com");
        result.Status.Should().Be("Sent");

        _mockEmailSender.Verify(s => s.SendAsync(message, It.IsAny<CancellationToken>()), Times.Once);
    }
}
