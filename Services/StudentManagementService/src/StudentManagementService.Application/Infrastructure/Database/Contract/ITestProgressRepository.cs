using StudentManagementService.Domain.Entities;

namespace StudentManagementService.Application.Infrastructure.Database.Contract;

public interface ITestProgressRepository
{
    Task<TestProgress?> GetByIdAsync(Guid id, CancellationToken ct);

    Task<List<TestProgress>> GetByStudentIdAsync(Guid studentId, CancellationToken ct);

    Task<List<TestProgress>> GetByStudentAndTestAsync(Guid studentId, Guid testId, CancellationToken ct);

    Task<TestProgress?> GetLatestByStudentAndTestAsync(Guid studentId, Guid testId, CancellationToken ct);

    Task AddAsync(TestProgress progress, CancellationToken ct);

    Task UpdateAsync(TestProgress progress, CancellationToken ct);
}
