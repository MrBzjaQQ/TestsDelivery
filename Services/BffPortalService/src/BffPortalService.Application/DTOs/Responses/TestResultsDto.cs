namespace BffPortalService.Application.DTOs.Responses;

public record TestResultsDto
{
    public Guid TestId { get; init; }

    public string TestTitle { get; init; } = string.Empty;

    public int Score { get; init; }

    public int MaxScore { get; init; }

    public double Percentage { get; init; }

    public bool IsPassed { get; init; }

    public int PassedThreshold { get; init; }

    public int AttemptNumber { get; init; }

    public DateTime CompletedAt { get; init; }

    public List<AnswerResultDto> Answers { get; init; } = [];

    public bool CanRetake { get; init; }

    public int AttemptsRemaining { get; init; }
}

public record AnswerResultDto
{
    public Guid QuestionId { get; init; }

    public string QuestionText { get; init; } = string.Empty;

    public List<Guid> SelectedOptionIds { get; init; } = [];

    public bool IsCorrect { get; init; }

    public int PointsEarned { get; init; }

    public List<Guid> CorrectOptionIds { get; init; } = [];

    public string? TextAnswer { get; init; }

    public string? Feedback { get; init; }
}
