using QuestionManagementService.Domain.Entities;

namespace QuestionManagementService.Application.Infrastructure.Database.Contract;

public interface IQuestionRepository
{
    Task<Question?> GetByIdAsync(Guid id, CancellationToken ct);

    Task<List<Question>> GetAllAsync(CancellationToken ct);

    Task<List<Question>> GetByFilterAsync(string? category, string? difficulty, Guid? questionBankId, CancellationToken ct);

    Task<List<Question>> GetByBankIdAsync(Guid bankId, CancellationToken ct);

    Task AddAsync(Question question, CancellationToken ct);

    Task UpdateAsync(Question question, CancellationToken ct);

    Task DeleteAsync(Guid id, CancellationToken ct);

    Task<int> GetCountByBankIdAsync(Guid bankId, CancellationToken ct);
}
