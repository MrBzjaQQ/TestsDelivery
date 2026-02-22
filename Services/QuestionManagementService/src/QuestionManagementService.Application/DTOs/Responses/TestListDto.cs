namespace QuestionManagementService.Application.DTOs.Responses;

public record TestListDto
{
    public List<TestDto> Tests { get; init; } = [];

    public int TotalCount { get; init; }
}
