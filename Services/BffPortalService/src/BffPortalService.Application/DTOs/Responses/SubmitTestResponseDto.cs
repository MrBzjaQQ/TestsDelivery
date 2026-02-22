namespace BffPortalService.Application.DTOs.Responses;

public record SubmitTestResponseDto
{
    public Guid TestAssignmentId { get; init; }

    public Guid TestId { get; init; }

    public Guid StudentId { get; init; }

    public string Status { get; init; } = string.Empty;

    public DateTime SubmittedAt { get; init; }

    public int AttemptNumber { get; init; }

    public int AnswersCount { get; init; }
}
