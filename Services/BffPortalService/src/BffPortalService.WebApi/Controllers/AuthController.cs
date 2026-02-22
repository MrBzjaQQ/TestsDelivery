using BffPortalService.Infrastructure.HttpClients.External.Clients;
using BffPortalService.WebApi.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BffPortalService.WebApi.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly IIdentityServiceClient _identityClient;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IIdentityServiceClient identityClient,
        ILogger<AuthController> logger)
    {
        _identityClient = identityClient;
        _logger = logger;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(ResponseResultModel<RegisterResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken ct)
    {
        _logger.LogInformation("Registering user with email: {Email}", request.Email);

        var response = await _identityClient.RegisterAsync(request);

        if (!response.IsSuccessStatusCode || response.Content?.Data is null)
        {
            return StatusCode((int)response.StatusCode, new ProblemDetails
            {
                Title = "Registration failed",
                Detail = response.Error?.Content?.ToString() ?? "Unable to register user",
                Status = (int)response.StatusCode
            });
        }

        var result = new ResponseResultModel<RegisterResponse>
        {
            IsError = false,
            Message = response.Content.Message,
            Data = response.Content.Data
        };

        return Ok(result);
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(ResponseResultModel<AuthResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        _logger.LogInformation("Login attempt for email: {Email}", request.Email);

        var response = await _identityClient.LoginAsync(request);

        if (!response.IsSuccessStatusCode || response.Content?.Data is null)
        {
            return StatusCode((int)response.StatusCode, new ProblemDetails
            {
                Title = "Login failed",
                Detail = response.Error?.Content?.ToString() ?? "Invalid credentials",
                Status = (int)response.StatusCode
            });
        }

        var result = new ResponseResultModel<AuthResponse>
        {
            IsError = false,
            Message = response.Content.Message,
            Data = response.Content.Data
        };

        return Ok(result);
    }

    [HttpPost("refresh")]
    [ProducesResponseType(typeof(ResponseResultModel<TokenResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request, CancellationToken ct)
    {
        _logger.LogInformation("Refreshing token");

        var response = await _identityClient.RefreshTokenAsync(request);

        if (!response.IsSuccessStatusCode || response.Content?.Data is null)
        {
            return StatusCode((int)response.StatusCode, new ProblemDetails
            {
                Title = "Token refresh failed",
                Detail = response.Error?.Content?.ToString() ?? "Invalid refresh token",
                Status = (int)response.StatusCode
            });
        }

        var result = new ResponseResultModel<TokenResponse>
        {
            IsError = false,
            Message = response.Content.Message,
            Data = response.Content.Data
        };

        return Ok(result);
    }

    [Authorize]
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Logout(CancellationToken ct)
    {
        var accessToken = GetAccessToken();

        _logger.LogInformation("Logging out user");

        var response = await _identityClient.LogoutAsync($"Bearer {accessToken}");

        if (!response.IsSuccessStatusCode)
        {
            return StatusCode((int)response.StatusCode, new ProblemDetails
            {
                Title = "Logout failed",
                Status = (int)response.StatusCode
            });
        }

        return NoContent();
    }

    [HttpPost("forgot-password")]
    [ProducesResponseType(typeof(ResponseResultModel<object>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request, CancellationToken ct)
    {
        _logger.LogInformation("Password reset requested for email: {Email}", request.Email);

        var response = await _identityClient.ForgotPasswordAsync(request);

        var result = new ResponseResultModel<object>
        {
            IsError = false,
            Message = response.Content?.Message ?? "Password reset email sent",
            Data = null
        };

        return Ok(result);
    }

    [HttpPost("reset-password")]
    [ProducesResponseType(typeof(ResponseResultModel<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request, CancellationToken ct)
    {
        _logger.LogInformation("Resetting password");

        var response = await _identityClient.ResetPasswordAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            return StatusCode((int)response.StatusCode, new ProblemDetails
            {
                Title = "Password reset failed",
                Detail = response.Error?.Content?.ToString() ?? "Invalid or expired token",
                Status = (int)response.StatusCode
            });
        }

        var result = new ResponseResultModel<object>
        {
            IsError = false,
            Message = response.Content?.Message ?? "Password reset successfully",
            Data = null
        };

        return Ok(result);
    }

    private string GetAccessToken()
    {
        var authorizationHeader = HttpContext.Request.Headers.Authorization.ToString();
        return authorizationHeader.Replace("Bearer ", string.Empty);
    }
}
