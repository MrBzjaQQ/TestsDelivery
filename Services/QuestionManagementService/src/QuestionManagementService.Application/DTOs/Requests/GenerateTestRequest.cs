namespace QuestionManagementService.Application.DTOs.Requests;

public record GenerateTestRequest
{
    public string Title { get; init; } = string.Empty;

    public int QuestionCount { get; init; }

    public string? DifficultyFilter { get; init; }
}
