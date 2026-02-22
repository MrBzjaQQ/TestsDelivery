using Microsoft.EntityFrameworkCore;
using StudentManagementService.Application.Infrastructure.Database.Contract;
using StudentManagementService.Domain.Entities;
using StudentManagementService.Infrastructure.Database.Context;

namespace StudentManagementService.Infrastructure.Database.Repositories;

public class TestProgressRepository : ITestProgressRepository
{
    private readonly IStudentDbContext _context;

    public TestProgressRepository(IStudentDbContext context)
    {
        _context = context;
    }

    public async Task<TestProgress?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _context.TestProgresses.FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public async Task<List<TestProgress>> GetByStudentIdAsync(Guid studentId, CancellationToken ct)
    {
        return await _context.TestProgresses
            .Where(p => p.StudentId == studentId)
            .ToListAsync(ct);
    }

    public async Task<List<TestProgress>> GetByStudentAndTestAsync(Guid studentId, Guid testId, CancellationToken ct)
    {
        return await _context.TestProgresses
            .Where(p => p.StudentId == studentId && p.TestId == testId)
            .OrderBy(p => p.AttemptNumber)
            .ToListAsync(ct);
    }

    public async Task<TestProgress?> GetLatestByStudentAndTestAsync(Guid studentId, Guid testId, CancellationToken ct)
    {
        return await _context.TestProgresses
            .Where(p => p.StudentId == studentId && p.TestId == testId)
            .OrderByDescending(p => p.AttemptNumber)
            .FirstOrDefaultAsync(ct);
    }

    public async Task AddAsync(TestProgress progress, CancellationToken ct)
    {
        await _context.TestProgresses.AddAsync(progress, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(TestProgress progress, CancellationToken ct)
    {
        _context.TestProgresses.Update(progress);
        await _context.SaveChangesAsync(ct);
    }
}
