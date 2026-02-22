using Microsoft.EntityFrameworkCore;
using TestCheckingService.Application.Infrastructure.Database.Contract;
using TestCheckingService.Domain.Entities;
using TestCheckingService.Infrastructure.Database.Context;

namespace TestCheckingService.Infrastructure.Database.Repositories;

public class TestRepository : ITestRepository
{
    private readonly ITestCheckingDbContext _context;

    public TestRepository(ITestCheckingDbContext context)
    {
        _context = context;
    }

    public async Task<Test?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _context.Tests
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id, ct);
    }

    public async Task AddAsync(Test test, CancellationToken ct)
    {
        await _context.Tests.AddAsync(test, ct);
        await _context.SaveChangesAsync(ct);
    }
}
