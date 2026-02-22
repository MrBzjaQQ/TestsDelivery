namespace TestCheckingService.Domain.Entities;

public class TestResult
{
    public Guid Id { get; init; }

    public Guid TestId { get; init; }

    public Guid StudentId { get; init; }

    public int Score { get; init; }

    public int MaxScore { get; init; }

    public decimal Percentage { get; init; }

    public bool IsPassed { get; init; }

    public int AttemptNumber { get; init; }

    public DateTime? PassedDate { get; init; }

    public string Answers { get; init; } = string.Empty;

    public DateTime CreatedAt { get; init; }
}
