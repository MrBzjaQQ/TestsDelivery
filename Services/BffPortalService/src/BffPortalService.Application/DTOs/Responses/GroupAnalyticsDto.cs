namespace BffPortalService.Application.DTOs.Responses;

public record GroupAnalyticsDto
{
    public Guid GroupId { get; init; }

    public string GroupName { get; init; } = string.Empty;

    public int TotalStudents { get; init; }

    public int ActiveStudents { get; init; }

    public int CompletedTests { get; init; }

    public double AverageScore { get; init; }

    public double PassRate { get; init; }

    public List<TopPerformerDto> TopPerformers { get; init; } = [];

    public GroupProgressDto? GroupProgress { get; init; }
}

public record TopPerformerDto
{
    public Guid StudentId { get; init; }

    public string StudentName { get; init; } = string.Empty;

    public double AverageScore { get; init; }
}

public record GroupProgressDto
{
    public int TestsCompleted { get; init; }

    public int TestsInProgress { get; init; }

    public int TotalTestsAssigned { get; init; }
}
