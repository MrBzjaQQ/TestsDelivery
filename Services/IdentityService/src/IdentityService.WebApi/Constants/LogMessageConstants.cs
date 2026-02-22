namespace IdentityService.WebApi.Constants;

public class LogMessageConstants
{
    public const string UserRegistered = "User {Email} registered successfully with ID {UserId}";
    public const string UserLogin = "User {Email} logged in successfully";
    public const string UserLogout = "User {UserId} logged out";
    public const string TokenRefreshed = "Token refreshed for user {UserId}";
    public const string PasswordResetRequested = "Password reset requested for {Email}";
    public const string PasswordResetCompleted = "Password reset completed for {Email}";
    public const string RoleAssigned = "Role {Role} assigned to user {UserId}";
}
