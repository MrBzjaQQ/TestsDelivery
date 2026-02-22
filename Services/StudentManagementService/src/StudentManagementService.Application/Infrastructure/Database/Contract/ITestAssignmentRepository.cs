using StudentManagementService.Domain.Entities;

namespace StudentManagementService.Application.Infrastructure.Database.Contract;

public interface ITestAssignmentRepository
{
    Task<TestAssignment?> GetByIdAsync(Guid id, CancellationToken ct);

    Task<TestAssignment?> GetByStudentAndTestAsync(Guid studentId, Guid testId, CancellationToken ct);

    Task<List<TestAssignment>> GetByStudentIdAsync(Guid studentId, CancellationToken ct);

    Task<List<TestAssignment>> GetActiveByStudentIdAsync(Guid studentId, CancellationToken ct);

    Task AddAsync(TestAssignment assignment, CancellationToken ct);

    Task UpdateAsync(TestAssignment assignment, CancellationToken ct);
}
