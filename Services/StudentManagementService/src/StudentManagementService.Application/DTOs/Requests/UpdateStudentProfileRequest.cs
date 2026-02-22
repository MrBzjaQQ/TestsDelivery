using StudentManagementService.Application.DTOs.Responses;

namespace StudentManagementService.Application.DTOs.Requests;

public class UpdateStudentProfileRequest
{
    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public string? PhoneNumber { get; set; }

    public StudentProfileDto? Profile { get; set; }
}
