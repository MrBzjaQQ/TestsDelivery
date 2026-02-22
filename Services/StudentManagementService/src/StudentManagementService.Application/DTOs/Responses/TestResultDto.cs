namespace StudentManagementService.Application.DTOs.Responses;

public class TestResultDto
{
    public Guid TestId { get; set; }

    public string? TestTitle { get; set; }

    public List<TestAttemptDto> Attempts { get; set; } = [];
}

public class TestAttemptDto
{
    public short AttemptNumber { get; set; }

    public short? Score { get; set; }

    public short MaxScore { get; set; }

    public bool IsPassed { get; set; }

    public DateTime? SubmittedAt { get; set; }

    public List<AnswerResultDto>? Answers { get; set; }
}

public class AnswerResultDto
{
    public Guid QuestionId { get; set; }

    public Guid? SelectedOptionId { get; set; }

    public bool? IsCorrect { get; set; }

    public short? PointsEarned { get; set; }
}
