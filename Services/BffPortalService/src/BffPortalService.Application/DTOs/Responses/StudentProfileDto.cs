namespace BffPortalService.Application.DTOs.Responses;

public record StudentProfileDto
{
    public Guid StudentId { get; init; }

    public Guid UserId { get; init; }

    public string FirstName { get; init; } = string.Empty;

    public string LastName { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public GroupDto? Group { get; init; }

    public StudentProfileDetailsDto? Profile { get; init; }

    public StudentStatisticsDto? Statistics { get; init; }

    public List<RecentTestDto> RecentTests { get; init; } = [];
}

public record GroupDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public int StartYear { get; init; }

    public int EndYear { get; init; }
}

public record StudentProfileDetailsDto
{
    public string? AvatarUrl { get; init; }

    public string? Bio { get; init; }
}

public record StudentStatisticsDto
{
    public int TotalTests { get; init; }

    public int CompletedTests { get; init; }

    public int InProgressTests { get; init; }

    public double AverageScore { get; init; }
}

public record RecentTestDto
{
    public Guid TestId { get; init; }

    public string TestTitle { get; init; } = string.Empty;

    public string Status { get; init; } = string.Empty;

    public int Score { get; init; }

    public int MaxScore { get; init; }

    public double Percentages { get; init; }

    public bool IsPassed { get; init; }

    public DateTime? CompletedAt { get; init; }
}
