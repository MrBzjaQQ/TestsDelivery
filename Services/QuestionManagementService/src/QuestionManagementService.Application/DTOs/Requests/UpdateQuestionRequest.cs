namespace QuestionManagementService.Application.DTOs.Requests;

public record UpdateQuestionRequest
{
    public string Text { get; init; } = string.Empty;

    public string Category { get; init; } = string.Empty;

    public string Difficulty { get; init; } = "Easy";

    public List<AnswerOptionRequest>? Options { get; init; }
}
