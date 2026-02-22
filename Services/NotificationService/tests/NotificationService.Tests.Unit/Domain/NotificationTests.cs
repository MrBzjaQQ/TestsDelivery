using FluentAssertions;
using NotificationService.Domain.Entities;
using NotificationService.Domain.ValueObjects;
using Xunit;

namespace NotificationService.Tests.Unit.Domain;

public class NotificationTests
{
    [Fact]
    public void Notification_Should_HaveCorrectDefaultValues()
    {
        var notification = new Notification();

        notification.Id.Should().Be(Guid.Empty);
        notification.To.Should().BeEmpty();
        notification.Subject.Should().BeEmpty();
        notification.TemplateName.Should().BeEmpty();
        notification.Type.Should().Be(default(NotificationType));
        notification.Status.Should().Be(default(NotificationStatus));
        notification.RetryCount.Should().Be(0);
        notification.MaxRetries.Should().Be(3);
    }

    [Fact]
    public void Notification_Should_AllowSettingProperties()
    {
        var id = Guid.NewGuid();
        var now = DateTime.UtcNow;

        var notification = new Notification
        {
            Id = id,
            To = "test@example.com",
            Subject = "Test Subject",
            TemplateName = "test-template",
            TemplateData = "{\"key\":\"value\"}",
            Type = NotificationType.Email,
            Status = NotificationStatus.Sent,
            RetryCount = 1,
            MaxRetries = 5,
            ErrorMessage = "Test error",
            HtmlBody = "<html></html>",
            CreatedAt = now,
            SentAt = now.AddMinutes(1),
            RelatedEntityId = Guid.NewGuid(),
            RelatedEntityType = "Test"
        };

        notification.Id.Should().Be(id);
        notification.To.Should().Be("test@example.com");
        notification.Subject.Should().Be("Test Subject");
        notification.TemplateName.Should().Be("test-template");
        notification.Type.Should().Be(NotificationType.Email);
        notification.Status.Should().Be(NotificationStatus.Sent);
        notification.RetryCount.Should().Be(1);
        notification.MaxRetries.Should().Be(5);
        notification.ErrorMessage.Should().Be("Test error");
        notification.CreatedAt.Should().Be(now);
        notification.SentAt.Should().Be(now.AddMinutes(1));
    }
}
