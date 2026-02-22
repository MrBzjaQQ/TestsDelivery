namespace IdentityService.Infrastructure.Database.Messages;

public interface UserRegisteredEvent
{
    Guid UserId { get; }

    string Email { get; }

    string FirstName { get; }

    string LastName { get; }

    string Role { get; }

    DateTime RegisteredAt { get; }
}

public class UserRegisteredEventMessage : UserRegisteredEvent
{
    public Guid UserId { get; set; }

    public string Email { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public DateTime RegisteredAt { get; set; }
}
