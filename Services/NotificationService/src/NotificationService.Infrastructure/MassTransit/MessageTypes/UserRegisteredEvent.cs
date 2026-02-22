namespace NotificationService.Infrastructure.MassTransit.MessageTypes;

public interface UserRegisteredEvent
{
    Guid UserId { get; }

    string Email { get; }

    string FirstName { get; }

    string LastName { get; }

    string Role { get; }

    DateTime RegisteredAt { get; }
}
