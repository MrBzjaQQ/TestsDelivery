namespace IdentityService.Domain.Exceptions;

public class TokenRefreshException : Exception
{
    public TokenRefreshException()
        : base("Invalid or expired refresh token")
    {
    }

    public TokenRefreshException(string message)
        : base(message)
    {
    }
}
