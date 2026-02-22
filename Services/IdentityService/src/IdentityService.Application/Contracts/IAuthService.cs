using IdentityService.Application.DTOs.Requests;
using IdentityService.Application.DTOs.Responses;

namespace IdentityService.Application.Contracts;

public interface IAuthService
{
    Task<RegisterResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken);

    Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken);

    Task<TokenResponse> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);

    Task LogoutAsync(string userId, CancellationToken cancellationToken);

    Task ForgotPasswordAsync(string email, CancellationToken cancellationToken);

    Task ResetPasswordAsync(string token, string newPassword, CancellationToken cancellationToken);
}
