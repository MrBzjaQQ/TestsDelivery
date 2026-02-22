using IdentityService.Application.Contracts;
using IdentityService.Application.DTOs.Requests;
using IdentityService.Application.DTOs.Responses;
using IdentityService.Domain.Exceptions;
using IdentityService.Domain.ValueObjects;
using IdentityService.Infrastructure.Database.Identity;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IdentityService.Infrastructure.Database.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ITokenService tokenService,
        IPublishEndpoint publishEndpoint,
        ILogger<AuthService> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task<RegisterResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            throw new DuplicateEmailException(request.Email);
        }

        var user = new ApplicationUser
        {
            Email = request.Email,
            UserName = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Role = request.Role.ToString(),
            EmailVerified = false,
            CreatedAt = DateTime.UtcNow,
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new ArgumentException($"Failed to create user: {errors}");
        }

        await _userManager.AddToRoleAsync(user, request.Role.ToString());

        _logger.LogInformation("User {Email} registered successfully with ID {UserId}", user.Email, user.Id);

        return new RegisterResponse
        {
            UserId = user.Id,
            Email = user.Email!,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = request.Role,
            EmailVerified = user.EmailVerified,
            CreatedAt = user.CreatedAt,
        };
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            throw new InvalidCredentialsException();
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);

        if (result.IsLockedOut)
        {
            throw new AccountLockedException(request.Email);
        }

        if (!result.Succeeded)
        {
            throw new InvalidCredentialsException();
        }

        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? user.Role;

        var tokens = await _tokenService.GenerateTokensAsync(user.Id, user.Email!, role, cancellationToken);

        _logger.LogInformation("User {Email} logged in successfully", user.Email);

        return new AuthResponse
        {
            AccessToken = tokens.AccessToken,
            RefreshToken = tokens.RefreshToken,
            ExpiresIn = tokens.ExpiresIn,
            User = new UserDto
            {
                Id = user.Id,
                Email = user.Email!,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = Enum.Parse<UserRole>(role),
                EmailVerified = user.EmailVerified,
                CreatedAt = user.CreatedAt,
            },
        };
    }

    public async Task<TokenResponse> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var tokens = await _tokenService.RefreshAccessTokenAsync(refreshToken, cancellationToken);
        return tokens;
    }

    public async Task LogoutAsync(string userId, CancellationToken cancellationToken)
    {
        await _tokenService.InvalidateRefreshTokenAsync(userId, cancellationToken);
        _logger.LogInformation("User {UserId} logged out", userId);
    }

    public async Task ForgotPasswordAsync(string email, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return;
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        _logger.LogInformation("Password reset requested for {Email}", email);
    }

    public async Task ResetPasswordAsync(string token, string newPassword, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users
            .FirstOrDefaultAsync(u => u.Email == token, cancellationToken);

        if (user == null)
        {
            throw new InvalidCredentialsException("Invalid reset token");
        }

        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new ArgumentException($"Failed to reset password: {errors}");
        }

        _logger.LogInformation("Password reset completed for {Email}", user.Email);
    }
}
