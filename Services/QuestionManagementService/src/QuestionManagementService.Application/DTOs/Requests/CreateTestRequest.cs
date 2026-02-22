namespace QuestionManagementService.Application.DTOs.Requests;

public record CreateTestRequest
{
    public string Title { get; init; } = string.Empty;

    public string? Description { get; init; }

    public Guid QuestionBankId { get; init; }

    public Guid? TemplateId { get; init; }

    public int DurationMinutes { get; init; }

    public byte PassingScore { get; init; }

    public int MaxAttempts { get; init; }
}
