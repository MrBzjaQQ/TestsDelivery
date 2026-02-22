using Microsoft.AspNetCore.Identity;

namespace IdentityService.Infrastructure.Database.Identity;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Role { get; set; } = "Student";

    public bool EmailVerified { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}
