namespace StudentManagementService.Application.DTOs.Requests;

public class GetStudentsRequest
{
    public Guid? GroupId { get; set; }

    public string? Status { get; set; }
}
