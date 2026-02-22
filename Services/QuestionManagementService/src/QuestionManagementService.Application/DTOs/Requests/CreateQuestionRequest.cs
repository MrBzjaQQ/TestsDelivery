namespace QuestionManagementService.Application.DTOs.Requests;

public record CreateQuestionRequest
{
    public string Text { get; init; } = string.Empty;

    public string Category { get; init; } = string.Empty;

    public string Difficulty { get; init; } = "Easy";

    public Guid? QuestionBankId { get; init; }

    public List<AnswerOptionRequest>? Options { get; init; }
}

public record AnswerOptionRequest
{
    public string Text { get; init; } = string.Empty;

    public bool IsCorrect { get; init; }
}
