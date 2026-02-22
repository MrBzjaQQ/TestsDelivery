namespace StudentManagementService.Application.DTOs.Requests;

public class AssignTestRequest
{
    public DateTime? Deadline { get; set; }

    public short AttemptsAllowed { get; set; } = 3;
}
