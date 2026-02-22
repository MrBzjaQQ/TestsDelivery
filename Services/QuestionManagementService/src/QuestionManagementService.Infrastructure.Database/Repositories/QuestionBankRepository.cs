using Microsoft.EntityFrameworkCore;
using QuestionManagementService.Application.Infrastructure.Database.Contract;
using QuestionManagementService.Domain.Entities;
using QuestionManagementService.Infrastructure.Database.Context;

namespace QuestionManagementService.Infrastructure.Database.Repositories;

public class QuestionBankRepository : IQuestionBankRepository
{
    private readonly IQuestionDbContext _context;

    public QuestionBankRepository(IQuestionDbContext context)
    {
        _context = context;
    }

    public async Task<QuestionBank?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _context.QuestionBanks
            .FirstOrDefaultAsync(qb => qb.Id == id, ct);
    }

    public async Task AddAsync(QuestionBank questionBank, CancellationToken ct)
    {
        await _context.QuestionBanks.AddAsync(questionBank, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<int> GetQuestionsCountAsync(Guid bankId, CancellationToken ct)
    {
        return await _context.Questions
            .Where(q => q.QuestionBankId == bankId)
            .CountAsync(ct);
    }
}
