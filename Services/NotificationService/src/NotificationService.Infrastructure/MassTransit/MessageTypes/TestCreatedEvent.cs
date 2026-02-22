namespace NotificationService.Infrastructure.MassTransit.MessageTypes;

public interface TestCreatedEvent
{
    Guid TestId { get; }

    string Title { get; }

    Guid StudentId { get; }

    string StudentName { get; }

    string StudentEmail { get; }

    string CreatedBy { get; }

    DateTime CreatedAt { get; }
}
