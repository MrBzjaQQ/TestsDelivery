namespace BffPortalService.Application.DTOs.Requests;

public record GetStudentProfileRequest
{
    public Guid StudentId { get; init; }
}
