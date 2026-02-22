using TestCheckingService.Domain.Entities;

namespace TestCheckingService.Application.Infrastructure.Database.Contract;

public interface ITestRepository
{
    Task<Test?> GetByIdAsync(Guid id, CancellationToken ct);

    Task AddAsync(Test test, CancellationToken ct);
}
