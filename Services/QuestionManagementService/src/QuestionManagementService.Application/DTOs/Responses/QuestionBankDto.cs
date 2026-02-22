namespace QuestionManagementService.Application.DTOs.Responses;

public record QuestionBankDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    public int QuestionsCount { get; init; }

    public DateTime CreatedAt { get; init; }
}
