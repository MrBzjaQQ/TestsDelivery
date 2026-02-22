using FluentAssertions;
using NotificationService.Infrastructure.Database.Context;
using NotificationService.Infrastructure.Database.Repositories;
using NotificationService.Domain.Entities;
using NotificationService.Domain.ValueObjects;
using NotificationService.Tests.Integration.TestInfrastructure;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace NotificationService.Tests.Integration.Services;

public class NotificationRepositoryIntegrationTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;

    public NotificationRepositoryIntegrationTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task AddAsync_Should_PersistNotification()
    {
        await using var context = await _fixture.CreateDbContextAsync();
        var repository = new NotificationRepository(context);

        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            To = "test@example.com",
            Subject = "Test Subject",
            TemplateName = "test-template",
            Type = NotificationType.Email,
            Status = NotificationStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        await repository.AddAsync(notification, CancellationToken.None);
        await context.SaveChangesAsync(CancellationToken.None);

        var saved = await context.Notifications.FirstOrDefaultAsync(n => n.Id == notification.Id);
        saved.Should().NotBeNull();
        saved!.To.Should().Be("test@example.com");
    }

    [Fact]
    public async Task GetByIdAsync_Should_ReturnNotification_When_Exists()
    {
        await using var context = await _fixture.CreateDbContextAsync();
        var repository = new NotificationRepository(context);

        var notificationId = Guid.NewGuid();
        var notification = new Notification
        {
            Id = notificationId,
            To = "getbyid@example.com",
            Subject = "Get By Id Test",
            TemplateName = "test-template",
            Type = NotificationType.Email,
            Status = NotificationStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        await repository.AddAsync(notification, CancellationToken.None);
        await context.SaveChangesAsync(CancellationToken.None);

        var result = await repository.GetByIdAsync(notificationId, CancellationToken.None);

        result.Should().NotBeNull();
        result!.Id.Should().Be(notificationId);
    }

    [Fact]
    public async Task GetPendingNotificationsAsync_Should_ReturnOnlyPendingAndRetrying()
    {
        await using var context = await _fixture.CreateDbContextAsync();
        var repository = new NotificationRepository(context);

        var pendingNotification = new Notification
        {
            Id = Guid.NewGuid(),
            To = "pending@example.com",
            Subject = "Pending",
            TemplateName = "test-template",
            Type = NotificationType.Email,
            Status = NotificationStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        var retryingNotification = new Notification
        {
            Id = Guid.NewGuid(),
            To = "retrying@example.com",
            Subject = "Retrying",
            TemplateName = "test-template",
            Type = NotificationType.Email,
            Status = NotificationStatus.Retrying,
            CreatedAt = DateTime.UtcNow
        };

        var sentNotification = new Notification
        {
            Id = Guid.NewGuid(),
            To = "sent@example.com",
            Subject = "Sent",
            TemplateName = "test-template",
            Type = NotificationType.Email,
            Status = NotificationStatus.Sent,
            CreatedAt = DateTime.UtcNow
        };

        await repository.AddAsync(pendingNotification, CancellationToken.None);
        await repository.AddAsync(retryingNotification, CancellationToken.None);
        await repository.AddAsync(sentNotification, CancellationToken.None);
        await context.SaveChangesAsync(CancellationToken.None);

        var result = await repository.GetPendingNotificationsAsync(CancellationToken.None);

        result.Should().Contain(n => n.Id == pendingNotification.Id);
        result.Should().Contain(n => n.Id == retryingNotification.Id);
        result.Should().NotContain(n => n.Id == sentNotification.Id);
    }
}
