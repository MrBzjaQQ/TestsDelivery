namespace QuestionManagementService.Application.DTOs.Requests;

public record CreateTestTemplateRequest
{
    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    public int DefaultDuration { get; init; }

    public byte DefaultPassingScore { get; init; }

    public string? Configuration { get; init; }
}
