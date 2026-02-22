using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using IdentityService.Application.Contracts;
using IdentityService.Application.DTOs.Responses;
using IdentityService.Application.Settings;
using IdentityService.Domain.Exceptions;
using IdentityService.Infrastructure.Database.Context;
using IdentityService.Infrastructure.Database.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace IdentityService.Infrastructure.Database.Services;

public class TokenService : ITokenService
{
    private readonly JwtSettings _jwtSettings;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IdentityDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TokenService(
        IOptions<JwtSettings> jwtSettings,
        UserManager<ApplicationUser> userManager,
        IdentityDbContext dbContext,
        IHttpContextAccessor httpContextAccessor)
    {
        _jwtSettings = jwtSettings.Value;
        _userManager = userManager;
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TokenResponse> GenerateTokensAsync(string userId, string email, string role, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new UserNotFoundException(userId);
        }

        var accessToken = GenerateAccessToken(user, role);
        var refreshToken = GenerateRefreshToken();

        user.RefreshTokens.Add(refreshToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new TokenResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token,
            ExpiresIn = _jwtSettings.ExpirationMinutes,
        };
    }

    public async Task<TokenResponse> RefreshAccessTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var storedToken = await _dbContext.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken, cancellationToken);

        if (storedToken == null || !storedToken.IsActive)
        {
            throw new TokenRefreshException("Invalid or expired refresh token");
        }

        var user = storedToken.User;
        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? "Student";

        var newAccessToken = GenerateAccessToken(user, role);
        var newRefreshToken = GenerateRefreshToken();

        storedToken.Revoked = DateTime.UtcNow;
        storedToken.RevokedByIp = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
        storedToken.ReplacedByToken = newRefreshToken.Token;

        user.RefreshTokens.Add(newRefreshToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new TokenResponse
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken.Token,
            ExpiresIn = _jwtSettings.ExpirationMinutes,
        };
    }

    public async Task InvalidateRefreshTokenAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return;
        }

        var activeTokens = user.RefreshTokens.Where(rt => rt.IsActive);
        foreach (var token in activeTokens)
        {
            token.Revoked = DateTime.UtcNow;
            token.RevokedByIp = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ValidateRefreshTokenAsync(string userId, string refreshToken, CancellationToken cancellationToken)
    {
        var storedToken = await _dbContext.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.UserId == userId && rt.Token == refreshToken, cancellationToken);

        return storedToken != null && storedToken.IsActive;
    }

    private string GenerateAccessToken(ApplicationUser user, string role)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Role, role),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private RefreshToken GenerateRefreshToken()
    {
        return new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            Expires = DateTime.UtcNow.AddHours(_jwtSettings.RefreshTokenExpirationHours),
            Created = DateTime.UtcNow,
            CreatedByIp = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString(),
        };
    }
}
