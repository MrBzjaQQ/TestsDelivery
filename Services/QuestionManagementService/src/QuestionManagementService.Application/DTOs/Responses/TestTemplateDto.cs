namespace QuestionManagementService.Application.DTOs.Responses;

public record TestTemplateDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    public int DefaultDuration { get; init; }

    public byte DefaultPassingScore { get; init; }

    public string? Configuration { get; init; }

    public DateTime CreatedAt { get; init; }
}
