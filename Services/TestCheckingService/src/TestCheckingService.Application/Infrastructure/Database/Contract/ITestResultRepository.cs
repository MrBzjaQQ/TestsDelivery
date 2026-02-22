using TestCheckingService.Domain.Entities;

namespace TestCheckingService.Application.Infrastructure.Database.Contract;

public interface ITestResultRepository
{
    Task<TestResult?> GetByIdAsync(Guid testId, Guid studentId, CancellationToken ct);

    Task<TestResult?> GetByIdWithAnswersAsync(Guid testId, Guid studentId, CancellationToken ct);

    Task<List<TestResult>> GetByTestIdAsync(Guid testId, CancellationToken ct);

    Task<List<TestResult>> GetByStudentIdAsync(Guid studentId, CancellationToken ct);

    Task AddAsync(TestResult testResult, CancellationToken ct);

    Task<int> GetAttemptCountAsync(Guid testId, Guid studentId, CancellationToken ct);
}
