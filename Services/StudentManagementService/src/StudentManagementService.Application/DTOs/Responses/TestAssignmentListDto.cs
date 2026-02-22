namespace StudentManagementService.Application.DTOs.Responses;

public class TestAssignmentListDto
{
    public List<TestAssignmentDto> Assignments { get; set; } = [];

    public int TotalCount { get; set; }
}
