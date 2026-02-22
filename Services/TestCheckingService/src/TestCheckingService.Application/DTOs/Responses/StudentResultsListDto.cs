namespace TestCheckingService.Application.DTOs.Responses;

public class StudentResultsListDto
{
    public Guid StudentId { get; init; }

    public string StudentName { get; init; } = string.Empty;

    public int TotalTests { get; init; }

    public int PassedTests { get; init; }

    public int FailedTests { get; init; }

    public double AverageScore { get; init; }

    public List<StudentResultItemDto> Results { get; init; } = [];
}

public class StudentResultItemDto
{
    public Guid TestId { get; init; }

    public string TestTitle { get; init; } = string.Empty;

    public int Score { get; init; }

    public int MaxScore { get; init; }

    public double Percentage { get; init; }

    public bool IsPassed { get; init; }

    public int AttemptNumber { get; init; }

    public DateTime CompletedAt { get; init; }
}
