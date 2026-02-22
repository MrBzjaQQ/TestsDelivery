using StudentManagementService.Domain.Entities;

namespace StudentManagementService.Application.Infrastructure.Database.Contract;

public interface IStudyGroupRepository
{
    Task<StudyGroup?> GetByIdAsync(Guid id, CancellationToken ct);

    Task AddAsync(StudyGroup group, CancellationToken ct);

    Task<List<Student>> GetStudentsByGroupIdAsync(Guid groupId, CancellationToken ct);
}
