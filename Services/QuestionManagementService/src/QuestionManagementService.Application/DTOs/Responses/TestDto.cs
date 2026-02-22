namespace QuestionManagementService.Application.DTOs.Responses;

public record TestDto
{
    public Guid Id { get; init; }

    public string Title { get; init; } = string.Empty;

    public string? Description { get; init; }

    public Guid QuestionBankId { get; init; }

    public Guid? TemplateId { get; init; }

    public int DurationMinutes { get; init; }

    public byte PassingScore { get; init; }

    public int MaxAttempts { get; init; }

    public string Status { get; init; } = string.Empty;

    public List<QuestionDto>? Questions { get; init; }

    public DateTime CreatedAt { get; init; }
}
