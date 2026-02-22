namespace StudentManagementService.Application.DTOs.Responses;

public class StudentListDto
{
    public List<StudentDto> Students { get; set; } = [];

    public int TotalCount { get; set; }
}
