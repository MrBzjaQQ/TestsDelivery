namespace BffPortalService.Application.DTOs.Responses;

public record StartTestResponseDto
{
    public Guid TestId { get; init; }

    public Guid StudentId { get; init; }

    public string Status { get; init; } = string.Empty;

    public DateTime StartedAt { get; init; }

    public DateTime Deadline { get; init; }

    public int AttemptsUsed { get; init; }

    public int QuestionsCount { get; init; }
}
