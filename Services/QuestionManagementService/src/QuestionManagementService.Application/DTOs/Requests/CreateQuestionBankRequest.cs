namespace QuestionManagementService.Application.DTOs.Requests;

public record CreateQuestionBankRequest
{
    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    public Guid OwnerId { get; init; }
}
