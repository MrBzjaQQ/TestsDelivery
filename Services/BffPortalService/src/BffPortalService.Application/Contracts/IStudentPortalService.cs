using BffPortalService.Application.DTOs.Responses;

namespace BffPortalService.Application.Contracts;

public interface IStudentPortalService
{
    Task<StudentProfileDto> GetStudentProfileAsync(Guid studentId, string accessToken, CancellationToken ct);

    Task InvalidateStudentCacheAsync(Guid studentId, CancellationToken ct);
}
