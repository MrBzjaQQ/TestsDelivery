namespace TestCheckingService.Application.MassTransit.Events;

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

    int PassedThreshold { get; }

    DateTime PassedDate { get; }

    int AttemptNumber { get; }
}

public class TestResultReceived : TestResultReceivedEvent
{
    public Guid TestId { get; init; }

    public Guid StudentId { get; init; }

    public string StudentName { get; init; } = string.Empty;

    public string StudentEmail { get; init; } = string.Empty;

    public int Score { get; init; }

    public int MaxScore { get; init; }

    public double Percentage { get; init; }

    public bool IsPassed { get; init; }

    public int PassedThreshold { get; init; }

    public DateTime PassedDate { get; init; }

    public int AttemptNumber { get; init; }
}
