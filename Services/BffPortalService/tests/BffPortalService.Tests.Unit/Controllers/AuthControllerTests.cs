using BffPortalService.Infrastructure.HttpClients.External.Clients;
using BffPortalService.WebApi.Controllers;
using BffPortalService.WebApi.Shared;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Refit;
using System.Net;
using Xunit;

namespace BffPortalService.Tests.Unit.Controllers;

public class AuthControllerTests
{
    private readonly Mock<IIdentityServiceClient> _identityClientMock;
    private readonly Mock<ILogger<AuthController>> _loggerMock;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _identityClientMock = new Mock<IIdentityServiceClient>();
        _loggerMock = new Mock<ILogger<AuthController>>();
        _controller = new AuthController(_identityClientMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Register_WhenSuccessful_ShouldReturnOkResult()
    {
        var request = new RegisterRequest
        {
            Email = "test@example.com",
            Password = "Password123!",
            FirstName = "John",
            LastName = "Doe",
            Role = "Student"
        };

        var expectedResponse = new RegisterResponse
        {
            UserId = Guid.NewGuid().ToString(),
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Role = "Student",
            EmailVerified = false,
            CreatedAt = DateTime.UtcNow
        };

        var apiResponse = CreateSuccessApiResponse(new IdentityResponse<RegisterResponse>
        {
            IsError = false,
            Message = "User registered successfully",
            Data = expectedResponse
        });

        _identityClientMock
            .Setup(x => x.RegisterAsync(request))
            .ReturnsAsync(apiResponse);

        var result = await _controller.Register(request, CancellationToken.None);

        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeOfType<ResponseResultModel<RegisterResponse>>().Subject;
        response.IsError.Should().BeFalse();
        response.Data.Should().NotBeNull();
        response.Data!.Email.Should().Be(request.Email);
    }

    [Fact]
    public async Task Register_WhenFailed_ShouldReturnError()
    {
        var request = new RegisterRequest
        {
            Email = "existing@example.com",
            Password = "Password123!",
            FirstName = "John",
            LastName = "Doe"
        };

        var apiResponse = CreateErrorApiResponse<IdentityResponse<RegisterResponse>>(HttpStatusCode.Conflict);

        _identityClientMock
            .Setup(x => x.RegisterAsync(request))
            .ReturnsAsync(apiResponse);

        var result = await _controller.Register(request, CancellationToken.None);

        var objectResult = result.Should().BeOfType<ObjectResult>().Subject;
        objectResult.StatusCode.Should().Be((int)HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Login_WhenSuccessful_ShouldReturnOkResult()
    {
        var request = new LoginRequest
        {
            Email = "test@example.com",
            Password = "Password123!"
        };

        var expectedResponse = new AuthResponse
        {
            AccessToken = "access-token",
            RefreshToken = "refresh-token",
            ExpiresIn = 3600,
            User = new UserDto
            {
                Id = Guid.NewGuid().ToString(),
                Email = request.Email,
                FirstName = "John",
                LastName = "Doe",
                Role = "Student"
            }
        };

        var apiResponse = CreateSuccessApiResponse(new IdentityResponse<AuthResponse>
        {
            IsError = false,
            Message = "Login successful",
            Data = expectedResponse
        });

        _identityClientMock
            .Setup(x => x.LoginAsync(request))
            .ReturnsAsync(apiResponse);

        var result = await _controller.Login(request, CancellationToken.None);

        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeOfType<ResponseResultModel<AuthResponse>>().Subject;
        response.IsError.Should().BeFalse();
        response.Data.Should().NotBeNull();
        response.Data!.AccessToken.Should().Be("access-token");
    }

    [Fact]
    public async Task Login_WhenInvalidCredentials_ShouldReturnError()
    {
        var request = new LoginRequest
        {
            Email = "test@example.com",
            Password = "wrong-password"
        };

        var apiResponse = CreateErrorApiResponse<IdentityResponse<AuthResponse>>(HttpStatusCode.Unauthorized);

        _identityClientMock
            .Setup(x => x.LoginAsync(request))
            .ReturnsAsync(apiResponse);

        var result = await _controller.Login(request, CancellationToken.None);

        var objectResult = result.Should().BeOfType<ObjectResult>().Subject;
        objectResult.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task RefreshToken_WhenSuccessful_ShouldReturnOkResult()
    {
        var request = new RefreshTokenRequest
        {
            RefreshToken = "valid-refresh-token"
        };

        var expectedResponse = new TokenResponse
        {
            AccessToken = "new-access-token",
            RefreshToken = "new-refresh-token",
            ExpiresIn = 3600
        };

        var apiResponse = CreateSuccessApiResponse(new IdentityResponse<TokenResponse>
        {
            IsError = false,
            Message = "Token refreshed",
            Data = expectedResponse
        });

        _identityClientMock
            .Setup(x => x.RefreshTokenAsync(request))
            .ReturnsAsync(apiResponse);

        var result = await _controller.RefreshToken(request, CancellationToken.None);

        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeOfType<ResponseResultModel<TokenResponse>>().Subject;
        response.IsError.Should().BeFalse();
        response.Data.Should().NotBeNull();
        response.Data!.AccessToken.Should().Be("new-access-token");
    }

    [Fact]
    public async Task RefreshToken_WhenInvalid_ShouldReturnError()
    {
        var request = new RefreshTokenRequest
        {
            RefreshToken = "invalid-refresh-token"
        };

        var apiResponse = CreateErrorApiResponse<IdentityResponse<TokenResponse>>(HttpStatusCode.Unauthorized);

        _identityClientMock
            .Setup(x => x.RefreshTokenAsync(request))
            .ReturnsAsync(apiResponse);

        var result = await _controller.RefreshToken(request, CancellationToken.None);

        var objectResult = result.Should().BeOfType<ObjectResult>().Subject;
        objectResult.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Logout_WhenSuccessful_ShouldReturnNoContent()
    {
        SetupControllerContext(_controller);

        var apiResponse = CreateSuccessApiResponse(new HttpResponseMessage(HttpStatusCode.NoContent));

        _identityClientMock
            .Setup(x => x.LogoutAsync("Bearer test-token"))
            .ReturnsAsync(apiResponse);

        var result = await _controller.Logout(CancellationToken.None);

        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Logout_WhenFailed_ShouldReturnError()
    {
        SetupControllerContext(_controller);

        var apiResponse = CreateErrorApiResponse<HttpResponseMessage>(HttpStatusCode.Unauthorized);

        _identityClientMock
            .Setup(x => x.LogoutAsync("Bearer test-token"))
            .ReturnsAsync(apiResponse);

        var result = await _controller.Logout(CancellationToken.None);

        var objectResult = result.Should().BeOfType<ObjectResult>().Subject;
        objectResult.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task ForgotPassword_WhenSuccessful_ShouldReturnOkResult()
    {
        var request = new ForgotPasswordRequest
        {
            Email = "test@example.com"
        };

        var apiResponse = CreateSuccessApiResponse(new IdentityResponse<object?>
        {
            IsError = false,
            Message = "Password reset email sent",
            Data = null
        });

        _identityClientMock
            .Setup(x => x.ForgotPasswordAsync(request))
            .ReturnsAsync(apiResponse);

        var result = await _controller.ForgotPassword(request, CancellationToken.None);

        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeOfType<ResponseResultModel<object>>().Subject;
        response.IsError.Should().BeFalse();
        response.Message.Should().Be("Password reset email sent");
    }

    [Fact]
    public async Task ResetPassword_WhenSuccessful_ShouldReturnOkResult()
    {
        var request = new ResetPasswordRequest
        {
            Token = "reset-token",
            NewPassword = "NewPassword123!"
        };

        var apiResponse = CreateSuccessApiResponse(new IdentityResponse<object?>
        {
            IsError = false,
            Message = "Password reset successfully",
            Data = null
        });

        _identityClientMock
            .Setup(x => x.ResetPasswordAsync(request))
            .ReturnsAsync(apiResponse);

        var result = await _controller.ResetPassword(request, CancellationToken.None);

        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeOfType<ResponseResultModel<object>>().Subject;
        response.IsError.Should().BeFalse();
        response.Message.Should().Be("Password reset successfully");
    }

    [Fact]
    public async Task ResetPassword_WhenInvalidToken_ShouldReturnError()
    {
        var request = new ResetPasswordRequest
        {
            Token = "invalid-token",
            NewPassword = "NewPassword123!"
        };

        var apiResponse = CreateErrorApiResponse<IdentityResponse<object?>>(HttpStatusCode.BadRequest);

        _identityClientMock
            .Setup(x => x.ResetPasswordAsync(request))
            .ReturnsAsync(apiResponse);

        var result = await _controller.ResetPassword(request, CancellationToken.None);

        var objectResult = result.Should().BeOfType<ObjectResult>().Subject;
        objectResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
    }

    private static void SetupControllerContext(ControllerBase controller)
    {
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        controller.HttpContext.Request.Headers.Authorization = "Bearer test-token";
    }

    private static IApiResponse<T> CreateSuccessApiResponse<T>(T content) where T : class
    {
        var mockResponse = new Mock<IApiResponse<T>>();
        mockResponse.Setup(x => x.IsSuccessStatusCode).Returns(true);
        mockResponse.Setup(x => x.StatusCode).Returns(HttpStatusCode.OK);
        mockResponse.Setup(x => x.Content).Returns(content);
        return mockResponse.Object;
    }

    private static IApiResponse<T> CreateErrorApiResponse<T>(HttpStatusCode statusCode) where T : class
    {
        var mockResponse = new Mock<IApiResponse<T>>();
        mockResponse.Setup(x => x.IsSuccessStatusCode).Returns(false);
        mockResponse.Setup(x => x.StatusCode).Returns(statusCode);
        mockResponse.Setup(x => x.Content).Returns((T?)null);
        mockResponse.Setup(x => x.Error).Returns((ApiException?)null);
        return mockResponse.Object;
    }
}
