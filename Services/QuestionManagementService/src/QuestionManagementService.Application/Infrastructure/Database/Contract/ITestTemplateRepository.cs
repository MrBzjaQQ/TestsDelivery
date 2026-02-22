using QuestionManagementService.Domain.Entities;

namespace QuestionManagementService.Application.Infrastructure.Database.Contract;

public interface ITestTemplateRepository
{
    Task<TestTemplate?> GetByIdAsync(Guid id, CancellationToken ct);

    Task AddAsync(TestTemplate template, CancellationToken ct);

    Task UpdateAsync(TestTemplate template, CancellationToken ct);
}
