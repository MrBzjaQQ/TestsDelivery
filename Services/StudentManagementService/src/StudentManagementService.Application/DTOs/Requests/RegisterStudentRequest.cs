namespace StudentManagementService.Application.DTOs.Requests;

public class RegisterStudentRequest
{
    public Guid UserId { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public required string Email { get; set; }

    public Guid? GroupId { get; set; }

    public DateTime? EnrollmentDate { get; set; }
}
