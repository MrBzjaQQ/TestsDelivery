using QuestionManagementService.Application.Infrastructure.Database.Contract;
using QuestionManagementService.Infrastructure.Database.Context;

namespace QuestionManagementService.Infrastructure.Database.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly IQuestionDbContext _context;

    public UnitOfWork(IQuestionDbContext context)
    {
        _context = context;
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct)
    {
        return await _context.SaveChangesAsync(ct);
    }
}
