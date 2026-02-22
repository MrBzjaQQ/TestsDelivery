using QuestionManagementService.Domain.Entities;

namespace QuestionManagementService.Application.Infrastructure.Database.Contract;

public interface IQuestionBankRepository
{
    Task<QuestionBank?> GetByIdAsync(Guid id, CancellationToken ct);

    Task AddAsync(QuestionBank questionBank, CancellationToken ct);

    Task<int> GetQuestionsCountAsync(Guid bankId, CancellationToken ct);
}
