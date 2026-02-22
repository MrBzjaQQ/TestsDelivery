using FluentAssertions;
using NotificationService.Domain.Exceptions;
using NotificationService.Domain.ValueObjects;
using Xunit;

namespace NotificationService.Tests.Unit.Domain;

public class EmailAddressTests
{
    [Fact]
    public void Constructor_Should_CreateEmailAddress_When_Valid()
    {
        var email = "test@example.com";

        var emailAddress = new EmailAddress(email);

        emailAddress.Value.Should().Be(email);
    }

    [Theory]
    [InlineData("test@example.com")]
    [InlineData("user.name@example.org")]
    [InlineData("test123@test-domain.com")]
    public void IsValidEmail_Should_ReturnTrue_When_Valid(string email)
    {
        var result = EmailAddress.IsValidEmail(email);

        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("invalid")]
    [InlineData("invalid@")]
    [InlineData("@example.com")]
    [InlineData("test@")]
    public void IsValidEmail_Should_ReturnFalse_When_Invalid(string email)
    {
        var result = EmailAddress.IsValidEmail(email);

        result.Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_ThrowArgumentException_When_EmailIsEmpty()
    {
        var act = () => new EmailAddress(string.Empty);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Constructor_Should_ThrowArgumentException_When_EmailIsInvalid()
    {
        var act = () => new EmailAddress("invalid-email");

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ImplicitOperator_Should_ReturnStringValue()
    {
        var emailAddress = new EmailAddress("test@example.com");

        string result = emailAddress;

        result.Should().Be("test@example.com");
    }

    [Fact]
    public void ToString_Should_ReturnValue()
    {
        var emailAddress = new EmailAddress("test@example.com");

        var result = emailAddress.ToString();

        result.Should().Be("test@example.com");
    }
}
