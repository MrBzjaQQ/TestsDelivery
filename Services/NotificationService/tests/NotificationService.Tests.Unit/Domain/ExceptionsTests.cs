using FluentAssertions;
using NotificationService.Domain.Exceptions;
using Xunit;

namespace NotificationService.Tests.Unit.Domain;

public class ExceptionsTests
{
    [Fact]
    public void NotificationNotFoundException_Should_HaveCorrectProperties()
    {
        var notificationId = Guid.NewGuid();

        var exception = new NotificationNotFoundException(notificationId);

        exception.NotificationId.Should().Be(notificationId);
        exception.Message.Should().Contain(notificationId.ToString());
    }

    [Fact]
    public void EmailSendFailedException_Should_HaveCorrectProperties()
    {
        var email = "test@example.com";
        var innerException = new Exception("Inner error");

        var exception = new EmailSendFailedException(email, innerException);

        exception.EmailAddress.Should().Be(email);
        exception.Message.Should().Contain(email);
        exception.InnerException.Should().Be(innerException);
    }
}
