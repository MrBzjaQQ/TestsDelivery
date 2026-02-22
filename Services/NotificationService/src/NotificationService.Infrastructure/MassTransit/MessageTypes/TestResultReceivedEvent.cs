namespace NotificationService.Infrastructure.MassTransit.MessageTypes;

public interface TestResultReceivedEvent
{
    Guid TestId { get; }

    Guid StudentId { get; }

    string StudentName { get; }

    string StudentEmail { get; }

    int Score { get; }

    int MaxScore { get; }

    double Percentage { get; }

    bool IsPassed { get; }

    DateTime PassedDate { get; }
}
