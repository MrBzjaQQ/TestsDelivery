using BffPortalService.Application.DTOs.Responses;
using Refit;

namespace BffPortalService.Infrastructure.HttpClients.External.Clients;

public interface IStudentServiceClient
{
    [Get("/api/v1/students/{studentId}")]
    Task<IApiResponse<StudentProfileDto>> GetStudentAsync(Guid studentId, [Header("Authorization")] string authorization);

    [Get("/api/v1/groups/{groupId}")]
    Task<IApiResponse<GroupDto>> GetGroupAsync(Guid groupId, [Header("Authorization")] string authorization);

    [Get("/api/v1/groups/{groupId}/students")]
    Task<IApiResponse<List<StudentDto>>> GetGroupStudentsAsync(Guid groupId, [Header("Authorization")] string authorization);
}

public record StudentDto
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public Guid GroupId { get; init; }
    public bool IsActive { get; init; }
}
