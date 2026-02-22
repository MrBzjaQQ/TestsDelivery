namespace StudentManagementService.Domain.Entities;

public class Student
{
    public Guid Id { get; init; }

    public Guid UserId { get; init; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public Guid? GroupId { get; set; }

    public string? PhoneNumber { get; set; }

    public StudentProfile? Profile { get; set; }

    public StudentStatus Status { get; set; } = StudentStatus.Active;

    public DateTime EnrollmentDate { get; init; }

    public DateTime CreatedAt { get; init; }
}
