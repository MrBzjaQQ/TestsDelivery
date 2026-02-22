namespace IdentityService.Domain.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException(string userId)
        : base($"User with ID '{userId}' was not found.")
    {
        UserId = userId;
    }

    public UserNotFoundException(string email, bool byEmail)
        : base($"User with email '{email}' was not found.")
    {
        Email = email;
    }

    public string? UserId { get; }

    public string? Email { get; }
}
