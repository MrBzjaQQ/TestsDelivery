using Refit;

namespace BffPortalService.Infrastructure.HttpClients.External.Clients;

public interface IIdentityServiceClient
{
    [Post("/api/v1/auth/register")]
    Task<IApiResponse<IdentityResponse<RegisterResponse>>> RegisterAsync([Body] RegisterRequest request);

    [Post("/api/v1/auth/login")]
    Task<IApiResponse<IdentityResponse<AuthResponse>>> LoginAsync([Body] LoginRequest request);

    [Post("/api/v1/auth/refresh")]
    Task<IApiResponse<IdentityResponse<TokenResponse>>> RefreshTokenAsync([Body] RefreshTokenRequest request);

    [Post("/api/v1/auth/logout")]
    Task<IApiResponse<HttpResponseMessage>> LogoutAsync([Header("Authorization")] string authorization);

    [Post("/api/v1/auth/forgot-password")]
    Task<IApiResponse<IdentityResponse<object?>>> ForgotPasswordAsync([Body] ForgotPasswordRequest request);

    [Post("/api/v1/auth/reset-password")]
    Task<IApiResponse<IdentityResponse<object?>>> ResetPasswordAsync([Body] ResetPasswordRequest request);
}

public class IdentityResponse<T>
{
    public bool IsError { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
}

public record RegisterRequest
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Role { get; init; } = "Student";
}

public record LoginRequest
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}

public record RefreshTokenRequest
{
    public string RefreshToken { get; init; } = string.Empty;
}

public record ForgotPasswordRequest
{
    public string Email { get; init; } = string.Empty;
}

public record ResetPasswordRequest
{
    public string Token { get; init; } = string.Empty;
    public string NewPassword { get; init; } = string.Empty;
}

public record RegisterResponse
{
    public string UserId { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
    public bool EmailVerified { get; init; }
    public DateTime CreatedAt { get; init; }
}

public record AuthResponse
{
    public string AccessToken { get; init; } = string.Empty;
    public string RefreshToken { get; init; } = string.Empty;
    public int ExpiresIn { get; init; }
    public UserDto? User { get; init; }
}

public record TokenResponse
{
    public string AccessToken { get; init; } = string.Empty;
    public string RefreshToken { get; init; } = string.Empty;
    public int ExpiresIn { get; init; }
}

public record UserDto
{
    public string Id { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
    public bool EmailVerified { get; init; }
    public DateTime CreatedAt { get; init; }
}
