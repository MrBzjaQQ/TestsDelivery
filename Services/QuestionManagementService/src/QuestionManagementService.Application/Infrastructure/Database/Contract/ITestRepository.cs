using QuestionManagementService.Domain.Entities;

namespace QuestionManagementService.Application.Infrastructure.Database.Contract;

public interface ITestRepository
{
    Task<Test?> GetByIdAsync(Guid id, CancellationToken ct);

    Task<List<Test>> GetByTemplateIdAsync(Guid templateId, CancellationToken ct);

    Task AddAsync(Test test, CancellationToken ct);
}
