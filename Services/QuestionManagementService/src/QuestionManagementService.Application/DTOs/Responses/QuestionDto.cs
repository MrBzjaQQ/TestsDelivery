namespace QuestionManagementService.Application.DTOs.Responses;

public record QuestionDto
{
    public Guid Id { get; init; }

    public string Text { get; init; } = string.Empty;

    public string Category { get; init; } = string.Empty;

    public string Difficulty { get; init; } = string.Empty;

    public Guid? QuestionBankId { get; init; }

    public List<AnswerOptionDto>? Options { get; init; }

    public DateTime CreatedAt { get; init; }
}

public record AnswerOptionDto
{
    public string Text { get; init; } = string.Empty;

    public bool IsCorrect { get; init; }
}
