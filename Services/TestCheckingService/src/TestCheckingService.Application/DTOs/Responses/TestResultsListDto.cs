namespace TestCheckingService.Application.DTOs.Responses;

public class TestResultsListDto
{
    public Guid TestId { get; init; }

    public string TestTitle { get; init; } = string.Empty;

    public int TotalResults { get; init; }

    public int PassedResults { get; init; }

    public int FailedResults { get; init; }

    public double AverageScore { get; init; }

    public List<StudentTestResultDto> Results { get; init; } = [];
}

public class StudentTestResultDto
{
    public Guid StudentId { get; init; }

    public string StudentName { get; init; } = string.Empty;

    public int Score { get; init; }

    public int MaxScore { get; init; }

    public double Percentage { get; init; }

    public bool IsPassed { get; init; }

    public int AttemptNumber { get; init; }

    public DateTime CompletedAt { get; init; }
}
