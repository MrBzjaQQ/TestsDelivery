namespace TestCheckingService.Application.DTOs.Responses;

public class TestResultDto
{
    public Guid TestId { get; init; }

    public Guid StudentId { get; init; }

    public int Score { get; init; }

    public int MaxScore { get; init; }

    public double Percentage { get; init; }

    public bool IsPassed { get; init; }

    public int PassedThreshold { get; init; }

    public DateTime? PassedDate { get; init; }

    public int AttemptNumber { get; init; }

    public List<AnswerResultDto> AnswerResults { get; init; } = [];
}
