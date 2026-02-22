using FluentAssertions;
using IdentityService.Application.Contracts;
using IdentityService.Application.DTOs.Requests;
using IdentityService.Application.DTOs.Responses;
using IdentityService.Domain.Exceptions;
using IdentityService.Domain.ValueObjects;
using IdentityService.Infrastructure.Database.Identity;
using IdentityService.Infrastructure.Database.Services;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace IdentityService.Tests.Unit.Services;

public class AuthServiceTests
{
    private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
    private readonly Mock<SignInManager<ApplicationUser>> _mockSignInManager;
    private readonly Mock<ITokenService> _mockTokenService;
    private readonly Mock<IPublishEndpoint> _mockPublishEndpoint;
    private readonly Mock<ILogger<AuthService>> _mockLogger;

    public AuthServiceTests()
    {
        var userStore = new Mock<IUserStore<ApplicationUser>>();
        _mockUserManager = new Mock<UserManager<ApplicationUser>>(
            userStore.Object, null, null, null, null, null, null, null, null);

        var contextAccessor = new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
        var userClaimsPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();
        _mockSignInManager = new Mock<SignInManager<ApplicationUser>>(
            _mockUserManager.Object,
            contextAccessor.Object,
            userClaimsPrincipalFactory.Object,
            null, null, null, null);

        _mockTokenService = new Mock<ITokenService>();
        _mockPublishEndpoint = new Mock<IPublishEndpoint>();
        _mockLogger = new Mock<ILogger<AuthService>>();
    }

    [Fact]
    public async Task RegisterAsync_ShouldThrowDuplicateEmailException_WhenEmailExists()
    {
        var existingUser = new ApplicationUser { Email = "test@example.com" };
        _mockUserManager.Setup(um => um.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(existingUser);

        var authService = CreateAuthService();

        var request = new RegisterRequest
        {
            Email = "test@example.com",
            Password = "Password123!",
            FirstName = "John",
            LastName = "Doe",
            Role = UserRole.Student,
        };

        var act = async () => await authService.RegisterAsync(request, CancellationToken.None);

        await act.Should().ThrowAsync<DuplicateEmailException>();
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturnRegisterResponse_WhenUserCreated()
    {
        _mockUserManager.Setup(um => um.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((ApplicationUser?)null);
        _mockUserManager.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
        _mockUserManager.Setup(um => um.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        var authService = CreateAuthService();

        var request = new RegisterRequest
        {
            Email = "test@example.com",
            Password = "Password123!",
            FirstName = "John",
            LastName = "Doe",
            Role = UserRole.Student,
        };

        var result = await authService.RegisterAsync(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Email.Should().Be("test@example.com");
        result.FirstName.Should().Be("John");
        result.LastName.Should().Be("Doe");
        result.Role.Should().Be(UserRole.Student);
    }

    [Fact]
    public async Task LoginAsync_ShouldThrowInvalidCredentialsException_WhenUserNotFound()
    {
        _mockUserManager.Setup(um => um.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((ApplicationUser?)null);

        var authService = CreateAuthService();

        var request = new LoginRequest
        {
            Email = "test@example.com",
            Password = "Password123!",
        };

        var act = async () => await authService.LoginAsync(request, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidCredentialsException>();
    }

    private AuthService CreateAuthService()
    {
        _mockTokenService.Setup(ts => ts.GenerateTokensAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new TokenResponse { AccessToken = "token", RefreshToken = "refresh", ExpiresIn = 60 });

        return new AuthService(
            _mockUserManager.Object,
            _mockSignInManager.Object,
            _mockTokenService.Object,
            _mockPublishEndpoint.Object,
            _mockLogger.Object);
    }
}
