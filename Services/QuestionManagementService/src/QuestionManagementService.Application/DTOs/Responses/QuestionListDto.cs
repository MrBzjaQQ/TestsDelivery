namespace QuestionManagementService.Application.DTOs.Responses;

public record QuestionListDto
{
    public List<QuestionDto> Questions { get; init; } = [];

    public int TotalCount { get; init; }
}
