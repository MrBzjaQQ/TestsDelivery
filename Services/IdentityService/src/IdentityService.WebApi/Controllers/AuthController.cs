using IdentityService.Application.Contracts;
using IdentityService.Application.DTOs.Requests;
using IdentityService.Application.DTOs.Responses;
using IdentityService.WebApi.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IdentityService.WebApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(ResponseResultModel<RegisterResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.RegisterAsync(request, cancellationToken);

        var response = new ResponseResultModel<RegisterResponse>
        {
            IsError = false,
            Message = "User registered successfully. Please verify your email.",
            Data = result,
        };

        return CreatedAtAction(nameof(GetUser), "Users", new { id = result.UserId }, response);
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(ResponseResultModel<AuthResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.LoginAsync(request, cancellationToken);

        var response = new ResponseResultModel<AuthResponse>
        {
            IsError = false,
            Message = "Login successful",
            Data = result,
        };

        return Ok(response);
    }

    [HttpPost("refresh")]
    [ProducesResponseType(typeof(ResponseResultModel<TokenResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.RefreshTokenAsync(request.RefreshToken, cancellationToken);

        var response = new ResponseResultModel<TokenResponse>
        {
            IsError = false,
            Message = "Tokens refreshed",
            Data = result,
        };

        return Ok(response);
    }

    [Authorize]
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        await _authService.LogoutAsync(userId, cancellationToken);

        return NoContent();
    }

    [HttpPost("forgot-password")]
    [ProducesResponseType(typeof(ResponseResultModel<object>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request, CancellationToken cancellationToken)
    {
        await _authService.ForgotPasswordAsync(request.Email, cancellationToken);

        var response = new ResponseResultModel<object>
        {
            IsError = false,
            Message = "Password reset link sent to email",
            Data = null,
        };

        return Ok(response);
    }

    [HttpPost("reset-password")]
    [ProducesResponseType(typeof(ResponseResultModel<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request, CancellationToken cancellationToken)
    {
        await _authService.ResetPasswordAsync(request.Token, request.NewPassword, cancellationToken);

        var response = new ResponseResultModel<object>
        {
            IsError = false,
            Message = "Password reset successful",
            Data = null,
        };

        return Ok(response);
    }

    [NonAction]
    public IActionResult GetUser(string id)
    {
        return Ok();
    }
}
