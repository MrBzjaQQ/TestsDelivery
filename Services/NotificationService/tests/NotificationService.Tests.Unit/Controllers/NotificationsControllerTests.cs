using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NotificationService.Application.Contracts;
using NotificationService.Application.DTOs.Requests;
using NotificationService.Application.DTOs.Responses;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Exceptions;
using NotificationService.Domain.ValueObjects;
using NotificationService.WebApi.Controllers;
using NotificationService.WebApi.Shared;
using Xunit;

namespace NotificationService.Tests.Unit.Controllers;

public class NotificationsControllerTests
{
    private readonly Mock<IEmailNotificationService> _mockEmailService;
    private readonly Mock<INotificationQueueService> _mockQueueService;
    private readonly NotificationsController _controller;

    public NotificationsControllerTests()
    {
        _mockEmailService = new Mock<IEmailNotificationService>();
        _mockQueueService = new Mock<INotificationQueueService>();
        _controller = new NotificationsController(_mockEmailService.Object, _mockQueueService.Object);
    }

    [Fact]
    public async Task SendEmail_Should_ReturnOk_When_Successful()
    {
        var request = new SendEmailRequest
        {
            To = "test@example.com",
            Subject = "Test Subject",
            TemplateName = "test-created"
        };

        var sendResponse = new SendEmailResponse(
            Guid.NewGuid(),
            "test@example.com",
            "Test Subject",
            DateTime.UtcNow,
            "Sent");

        _mockEmailService.Setup(s => s.SendEmailAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(sendResponse);

        var result = await _controller.SendEmail(request, CancellationToken.None);

        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeOfType<ResponseResultModel<SendEmailResponse>>().Subject;
        response.IsError.Should().BeFalse();
        response.Message.Should().Be("Email sent successfully");
        response.Data!.To.Should().Be("test@example.com");
    }

    [Fact]
    public async Task SendTestCreatedNotification_Should_ReturnOk_When_Successful()
    {
        var testId = Guid.NewGuid();
        var request = new TestCreatedNotificationRequest
        {
            StudentId = Guid.NewGuid(),
            StudentEmail = "student@example.com",
            StudentName = "John Doe"
        };

        var sendResponse = new SendEmailResponse(
            Guid.NewGuid(),
            "student@example.com",
            "New Test Available",
            DateTime.UtcNow,
            "Sent");

        _mockEmailService.Setup(s => s.SendEmailAsync(It.IsAny<SendEmailRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(sendResponse);

        var result = await _controller.SendTestCreatedNotification(testId, request, CancellationToken.None);

        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeOfType<ResponseResultModel<TestCreatedNotificationResponse>>().Subject;
        response.IsError.Should().BeFalse();
        response.Data!.TestId.Should().Be(testId);
        response.Data.StudentId.Should().Be(request.StudentId);
    }

    [Fact]
    public async Task SendTestResultNotification_Should_ReturnOk_When_Successful()
    {
        var studentId = Guid.NewGuid();
        var testId = Guid.NewGuid();
        var request = new TestResultNotificationRequest
        {
            StudentEmail = "student@example.com",
            StudentName = "John Doe",
            TestTitle = "Math Test",
            Score = 85,
            MaxScore = 100,
            Percentage = 85.0,
            IsPassed = true
        };

        var sendResponse = new SendEmailResponse(
            Guid.NewGuid(),
            "student@example.com",
            "Test Result Available",
            DateTime.UtcNow,
            "Sent");

        _mockEmailService.Setup(s => s.SendEmailAsync(It.IsAny<SendEmailRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(sendResponse);

        var result = await _controller.SendTestResultNotification(studentId, testId, request, CancellationToken.None);

        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeOfType<ResponseResultModel<TestResultNotificationResponse>>().Subject;
        response.IsError.Should().BeFalse();
        response.Data!.StudentId.Should().Be(studentId);
        response.Data.TestId.Should().Be(testId);
        response.Data.Score.Should().Be(85);
    }

    [Fact]
    public async Task GetPending_Should_ReturnPendingNotifications()
    {
        var pendingResponse = new PendingNotificationsResponse(
            new List<NotificationStatusDto>
            {
                new(Guid.NewGuid(), "test1@example.com", "Subject 1", "template1", NotificationStatus.Pending, 0, DateTime.UtcNow, null, null),
                new(Guid.NewGuid(), "test2@example.com", "Subject 2", "template2", NotificationStatus.Retrying, 1, DateTime.UtcNow, null, null)
            },
            2);

        _mockQueueService.Setup(s => s.GetPendingNotificationsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(pendingResponse);

        var result = await _controller.GetPending(CancellationToken.None);

        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeOfType<ResponseResultModel<PendingNotificationsResponse>>().Subject;
        response.IsError.Should().BeFalse();
        response.Data!.TotalCount.Should().Be(2);
    }

    [Fact]
    public async Task GetById_Should_ReturnNotification_When_Exists()
    {
        var notificationId = Guid.NewGuid();
        var notification = new Notification
        {
            Id = notificationId,
            To = "test@example.com",
            Subject = "Test Subject",
            TemplateName = "test-template",
            Status = NotificationStatus.Sent,
            RetryCount = 0,
            CreatedAt = DateTime.UtcNow.AddDays(-1),
            SentAt = DateTime.UtcNow
        };

        _mockQueueService.Setup(s => s.GetByIdAsync(notificationId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(notification);

        var result = await _controller.GetById(notificationId, CancellationToken.None);

        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeOfType<ResponseResultModel<NotificationStatusDto>>().Subject;
        response.IsError.Should().BeFalse();
        response.Data!.Id.Should().Be(notificationId);
    }

    [Fact]
    public async Task GetById_Should_ThrowNotificationNotFoundException_When_NotExists()
    {
        var notificationId = Guid.NewGuid();

        _mockQueueService.Setup(s => s.GetByIdAsync(notificationId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Notification?)null);

        var act = async () => await _controller.GetById(notificationId, CancellationToken.None);

        await act.Should().ThrowAsync<NotificationNotFoundException>();
    }
}
