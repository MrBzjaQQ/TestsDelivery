namespace IdentityService.Infrastructure.Database.Identity;

public class RefreshToken
{
    public Guid Id { get; set; }

    public string Token { get; set; } = string.Empty;

    public string UserId { get; set; } = string.Empty;

    public DateTime Created { get; set; }

    public DateTime Expires { get; set; }

    public string? CreatedByIp { get; set; }

    public DateTime? Revoked { get; set; }

    public string? RevokedByIp { get; set; }

    public string? ReplacedByToken { get; set; }

    public bool IsExpired => DateTime.UtcNow >= Expires;

    public bool IsRevoked => Revoked != null;

    public bool IsActive => !IsRevoked && !IsExpired;

    public virtual ApplicationUser User { get; set; } = null!;
}
