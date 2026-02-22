using StudentManagementService.Application.DTOs.Responses;
using StudentManagementService.Domain.Entities;

namespace StudentManagementService.Application.Contracts;

public interface IGroupService
{
    Task<GroupStatisticsDto> GetGroupStatisticsAsync(Guid groupId, CancellationToken ct);

    Task<StudentListDto> GetGroupStudentsAsync(Guid groupId, CancellationToken ct);

    Task<StudyGroup?> GetGroupByIdAsync(Guid groupId, CancellationToken ct);
}
