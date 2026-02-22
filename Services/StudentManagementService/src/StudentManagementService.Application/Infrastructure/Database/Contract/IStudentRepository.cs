using StudentManagementService.Domain.Entities;

namespace StudentManagementService.Application.Infrastructure.Database.Contract;

public interface IStudentRepository
{
    Task<Student?> GetByIdAsync(Guid id, CancellationToken ct);

    Task<Student?> GetByUserIdAsync(Guid userId, CancellationToken ct);

    Task<List<Student>> GetByGroupIdAsync(Guid groupId, CancellationToken ct);

    Task<List<Student>> GetByStatusAsync(StudentStatus status, CancellationToken ct);

    Task<List<Student>> GetByGroupIdAndStatusAsync(Guid groupId, StudentStatus status, CancellationToken ct);

    Task AddAsync(Student student, CancellationToken ct);

    Task UpdateAsync(Student student, CancellationToken ct);
}
