using Microsoft.EntityFrameworkCore;
using TestCheckingService.Application.Infrastructure.Database.Contract;
using TestCheckingService.Domain.Entities;
using TestCheckingService.Infrastructure.Database.Context;

namespace TestCheckingService.Infrastructure.Database.Repositories;

public class TestResultRepository : ITestResultRepository
{
    private readonly ITestCheckingDbContext _context;

    public TestResultRepository(ITestCheckingDbContext context)
    {
        _context = context;
    }

    public async Task<TestResult?> GetByIdAsync(Guid testId, Guid studentId, CancellationToken ct)
    {
        return await _context.TestResults
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.TestId == testId && r.StudentId == studentId, ct);
    }

    public async Task<TestResult?> GetByIdWithAnswersAsync(Guid testId, Guid studentId, CancellationToken ct)
    {
        return await _context.TestResults
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.TestId == testId && r.StudentId == studentId, ct);
    }

    public async Task<List<TestResult>> GetByTestIdAsync(Guid testId, CancellationToken ct)
    {
        return await _context.TestResults
            .AsNoTracking()
            .Where(r => r.TestId == testId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task<List<TestResult>> GetByStudentIdAsync(Guid studentId, CancellationToken ct)
    {
        return await _context.TestResults
            .AsNoTracking()
            .Where(r => r.StudentId == studentId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task AddAsync(TestResult testResult, CancellationToken ct)
    {
        await _context.TestResults.AddAsync(testResult, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<int> GetAttemptCountAsync(Guid testId, Guid studentId, CancellationToken ct)
    {
        return await _context.TestResults
            .AsNoTracking()
            .CountAsync(r => r.TestId == testId && r.StudentId == studentId, ct);
    }
}
