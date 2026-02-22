namespace BffPortalService.Application.DTOs.Responses;

public record TestQuestionsDto
{
    public Guid TestId { get; init; }

    public string TestTitle { get; init; } = string.Empty;

    public string TestDescription { get; init; } = string.Empty;

    public int DurationMinutes { get; init; }

    public int PassingScore { get; init; }

    public List<TestQuestionDto> Questions { get; init; } = [];

    public int TotalQuestions { get; init; }

    public int TotalPoints { get; init; }
}

public record TestQuestionDto
{
    public Guid QuestionId { get; init; }

    public string Text { get; init; } = string.Empty;

    public string Category { get; init; } = string.Empty;

    public string Difficulty { get; init; } = string.Empty;

    public int Order { get; init; }

    public List<QuestionOptionDto> Options { get; init; } = [];

    public Guid? ImageFileId { get; init; }

    public string? AnswerType { get; init; }

    public int? MaxLength { get; init; }
}

public record QuestionOptionDto
{
    public Guid OptionId { get; init; }

    public string Text { get; init; } = string.Empty;

    public int Order { get; init; }
}
