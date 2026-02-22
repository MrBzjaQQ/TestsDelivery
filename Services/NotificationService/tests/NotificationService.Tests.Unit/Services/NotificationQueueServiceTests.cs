using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NotificationService.Application.Contracts;
using NotificationService.Application.DTOs.Responses;
using NotificationService.Application.Infrastructure.Database.Contract;
using NotificationService.Application.Services;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Exceptions;
using NotificationService.Domain.ValueObjects;
using Xunit;

namespace NotificationService.Tests.Unit.Services;

public class NotificationQueueServiceTests
{
    private readonly Mock<INotificationRepository> _mockRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<ILogger<NotificationQueueService>> _mockLogger;
    private readonly NotificationQueueService _service;

    public NotificationQueueServiceTests()
    {
        _mockRepository = new Mock<INotificationRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockLogger = new Mock<ILogger<NotificationQueueService>>();
        _service = new NotificationQueueService(
            _mockRepository.Object,
            _mockUnitOfWork.Object,
            _mockLogger.Object);
    }

    [Fact]
    public async Task EnqueueNotificationAsync_Should_SetPendingStatusAndAddNotification()
    {
        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            To = "test@example.com",
            Subject = "Test",
            TemplateName = "test-template",
            Type = NotificationType.Email,
            CreatedAt = DateTime.UtcNow
        };

        _mockRepository.Setup(r => r.AddAsync(It.IsAny<Notification>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var result = await _service.EnqueueNotificationAsync(notification, CancellationToken.None);

        result.Status.Should().Be(NotificationStatus.Pending);

        _mockRepository.Verify(r => r.AddAsync(notification, It.IsAny<CancellationToken>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_Should_ReturnNotification_When_Exists()
    {
        var notificationId = Guid.NewGuid();
        var notification = new Notification
        {
            Id = notificationId,
            To = "test@example.com",
            Subject = "Test",
            TemplateName = "test-template",
            Status = NotificationStatus.Pending
        };

        _mockRepository.Setup(r => r.GetByIdAsync(notificationId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(notification);

        var result = await _service.GetByIdAsync(notificationId, CancellationToken.None);

        result.Should().NotBeNull();
        result!.Id.Should().Be(notificationId);
    }

    [Fact]
    public async Task GetByIdAsync_Should_ReturnNull_When_NotExists()
    {
        var notificationId = Guid.NewGuid();

        _mockRepository.Setup(r => r.GetByIdAsync(notificationId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Notification?)null);

        var result = await _service.GetByIdAsync(notificationId, CancellationToken.None);

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetPendingNotificationsAsync_Should_ReturnPendingNotifications()
    {
        var notifications = new List<Notification>
        {
            new() { Id = Guid.NewGuid(), To = "test1@example.com", Subject = "Test 1", Status = NotificationStatus.Pending },
            new() { Id = Guid.NewGuid(), To = "test2@example.com", Subject = "Test 2", Status = NotificationStatus.Retrying }
        };

        _mockRepository.Setup(r => r.GetPendingNotificationsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(notifications);

        var result = await _service.GetPendingNotificationsAsync(CancellationToken.None);

        result.Should().NotBeNull();
        result.TotalCount.Should().Be(2);
        result.Notifications.Should().HaveCount(2);
    }

    [Fact]
    public async Task MarkAsSentAsync_Should_UpdateStatusToSent_When_NotificationExists()
    {
        var notificationId = Guid.NewGuid();
        var notification = new Notification
        {
            Id = notificationId,
            To = "test@example.com",
            Subject = "Test",
            Status = NotificationStatus.Pending
        };

        _mockRepository.Setup(r => r.GetByIdAsync(notificationId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(notification);

        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Notification>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        await _service.MarkAsSentAsync(notificationId, CancellationToken.None);

        notification.Status.Should().Be(NotificationStatus.Sent);
        notification.SentAt.Should().NotBeNull();

        _mockRepository.Verify(r => r.UpdateAsync(notification, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task MarkAsSentAsync_Should_ThrowNotificationNotFoundException_When_NotificationDoesNotExist()
    {
        var notificationId = Guid.NewGuid();

        _mockRepository.Setup(r => r.GetByIdAsync(notificationId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Notification?)null);

        var act = async () => await _service.MarkAsSentAsync(notificationId, CancellationToken.None);

        await act.Should().ThrowAsync<NotificationNotFoundException>();
    }

    [Fact]
    public async Task MarkAsFailedAsync_Should_SetStatusToFailed_When_MaxRetriesReached()
    {
        var notificationId = Guid.NewGuid();
        var notification = new Notification
        {
            Id = notificationId,
            To = "test@example.com",
            Subject = "Test",
            Status = NotificationStatus.Retrying,
            RetryCount = 3,
            MaxRetries = 3
        };

        _mockRepository.Setup(r => r.GetByIdAsync(notificationId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(notification);

        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Notification>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        await _service.MarkAsFailedAsync(notificationId, "Error message", CancellationToken.None);

        notification.Status.Should().Be(NotificationStatus.Failed);
        notification.ErrorMessage.Should().Be("Error message");
    }

    [Fact]
    public async Task MarkAsFailedAsync_Should_SetStatusToRetrying_When_RetriesLeft()
    {
        var notificationId = Guid.NewGuid();
        var notification = new Notification
        {
            Id = notificationId,
            To = "test@example.com",
            Subject = "Test",
            Status = NotificationStatus.Pending,
            RetryCount = 1,
            MaxRetries = 3
        };

        _mockRepository.Setup(r => r.GetByIdAsync(notificationId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(notification);

        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Notification>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        await _service.MarkAsFailedAsync(notificationId, "Error message", CancellationToken.None);

        notification.Status.Should().Be(NotificationStatus.Retrying);
        notification.ErrorMessage.Should().Be("Error message");
    }

    [Fact]
    public async Task IncrementRetryCountAsync_Should_IncrementRetryCount()
    {
        var notificationId = Guid.NewGuid();
        var notification = new Notification
        {
            Id = notificationId,
            To = "test@example.com",
            Subject = "Test",
            RetryCount = 0
        };

        _mockRepository.Setup(r => r.GetByIdAsync(notificationId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(notification);

        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Notification>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        await _service.IncrementRetryCountAsync(notificationId, CancellationToken.None);

        notification.RetryCount.Should().Be(1);
    }
}
