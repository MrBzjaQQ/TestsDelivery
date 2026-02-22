using StudentManagementService.Application.DTOs.Requests;
using StudentManagementService.Application.DTOs.Responses;
using StudentManagementService.Domain.Entities;

namespace StudentManagementService.Application.Contracts;

public interface IStudentService
{
    Task<StudentDto> RegisterStudentAsync(RegisterStudentRequest request, CancellationToken ct);

    Task<StudentDto> GetStudentByIdAsync(Guid id, CancellationToken ct);

    Task<StudentDto> UpdateStudentProfileAsync(Guid id, UpdateStudentProfileRequest request, CancellationToken ct);

    Task<StudentListDto> GetStudentsAsync(Guid? groupId, string? status, CancellationToken ct);
}
