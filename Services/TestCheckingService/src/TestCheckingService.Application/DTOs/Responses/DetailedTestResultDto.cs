namespace TestCheckingService.Application.DTOs.Responses;

public class DetailedTestResultDto
{
    public Guid TestId { get; init; }

    public Guid StudentId { get; init; }

    public string TestTitle { get; init; } = string.Empty;

    public int Score { get; init; }

    public int MaxScore { get; init; }

    public double Percentage { get; init; }

    public bool IsPassed { get; init; }

    public int AttemptNumber { get; init; }

    public DateTime? PassedDate { get; init; }

    public List<DetailedAnswerResultDto> Answers { get; init; } = [];
}

public class DetailedAnswerResultDto
{
    public Guid QuestionId { get; init; }

    public string QuestionText { get; init; } = string.Empty;

    public Guid? SelectedOptionId { get; init; }

    public bool IsSelectedCorrect { get; init; }

    public int PointsEarned { get; init; }
}
