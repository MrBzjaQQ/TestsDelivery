using Microsoft.EntityFrameworkCore;
using QuestionManagementService.Application.Infrastructure.Database.Contract;
using QuestionManagementService.Domain.Entities;
using QuestionManagementService.Infrastructure.Database.Context;

namespace QuestionManagementService.Infrastructure.Database.Repositories;

public class TestRepository : ITestRepository
{
    private readonly IQuestionDbContext _context;

    public TestRepository(IQuestionDbContext context)
    {
        _context = context;
    }

    public async Task<Test?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _context.Tests
            .Include(t => t.TestQuestions)
            .ThenInclude(tq => tq.Question)
            .ThenInclude(q => q!.AnswerOptions)
            .FirstOrDefaultAsync(t => t.Id == id, ct);
    }

    public async Task<List<Test>> GetByTemplateIdAsync(Guid templateId, CancellationToken ct)
    {
        return await _context.Tests
            .Where(t => t.TemplateId == templateId)
            .ToListAsync(ct);
    }

    public async Task AddAsync(Test test, CancellationToken ct)
    {
        await _context.Tests.AddAsync(test, ct);
        await _context.SaveChangesAsync(ct);
    }
}
