namespace StudentManagementService.Application.DTOs.Responses;

public class StudentDto
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public Guid? GroupId { get; set; }

    public string? PhoneNumber { get; set; }

    public StudentProfileDto? Profile { get; set; }

    public string Status { get; set; } = string.Empty;

    public DateTime EnrollmentDate { get; set; }
}
