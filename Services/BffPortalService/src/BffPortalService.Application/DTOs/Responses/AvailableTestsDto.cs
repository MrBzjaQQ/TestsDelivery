namespace BffPortalService.Application.DTOs.Responses;

public record AvailableTestsDto
{
    public List<AvailableTestDto> Tests { get; init; } = [];

    public int TotalCount { get; init; }
}

public record AvailableTestDto
{
    public Guid TestId { get; init; }

    public string TestTitle { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public int DurationMinutes { get; init; }

    public int PassingScore { get; init; }

    public int MaxAttempts { get; init; }

    public int AttemptsUsed { get; init; }

    public string Status { get; init; } = string.Empty;

    public DateTime? AvailableFrom { get; init; }

    public DateTime? AvailableUntil { get; init; }

    public int QuestionsCount { get; init; }

    public bool CanTake { get; init; }

    public int? Score { get; init; }

    public int? MaxScore { get; init; }

    public double? Percentage { get; init; }

    public bool? IsPassed { get; init; }

    public DateTime? CompletedAt { get; init; }
}
