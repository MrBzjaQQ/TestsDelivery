using IdentityService.Application.DTOs.Responses;

namespace IdentityService.Application.Contracts;

public interface ITokenService
{
    Task<TokenResponse> GenerateTokensAsync(string userId, string email, string role, CancellationToken cancellationToken);

    Task<TokenResponse> RefreshAccessTokenAsync(string refreshToken, CancellationToken cancellationToken);

    Task InvalidateRefreshTokenAsync(string userId, CancellationToken cancellationToken);

    Task<bool> ValidateRefreshTokenAsync(string userId, string refreshToken, CancellationToken cancellationToken);
}
