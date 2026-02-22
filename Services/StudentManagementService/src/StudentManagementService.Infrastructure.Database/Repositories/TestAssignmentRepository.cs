using Microsoft.EntityFrameworkCore;
using StudentManagementService.Application.Infrastructure.Database.Contract;
using StudentManagementService.Domain.Entities;
using StudentManagementService.Infrastructure.Database.Context;

namespace StudentManagementService.Infrastructure.Database.Repositories;

public class TestAssignmentRepository : ITestAssignmentRepository
{
    private readonly IStudentDbContext _context;

    public TestAssignmentRepository(IStudentDbContext context)
    {
        _context = context;
    }

    public async Task<TestAssignment?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _context.TestAssignments.FirstOrDefaultAsync(a => a.Id == id, ct);
    }

    public async Task<TestAssignment?> GetByStudentAndTestAsync(Guid studentId, Guid testId, CancellationToken ct)
    {
        return await _context.TestAssignments
            .FirstOrDefaultAsync(a => a.StudentId == studentId && a.TestId == testId, ct);
    }

    public async Task<List<TestAssignment>> GetByStudentIdAsync(Guid studentId, CancellationToken ct)
    {
        return await _context.TestAssignments
            .Where(a => a.StudentId == studentId)
            .ToListAsync(ct);
    }

    public async Task<List<TestAssignment>> GetActiveByStudentIdAsync(Guid studentId, CancellationToken ct)
    {
        return await _context.TestAssignments
            .Where(a => a.StudentId == studentId &&
                        (a.Status == TestAssignmentStatus.Assigned || a.Status == TestAssignmentStatus.InProgress))
            .ToListAsync(ct);
    }

    public async Task AddAsync(TestAssignment assignment, CancellationToken ct)
    {
        await _context.TestAssignments.AddAsync(assignment, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(TestAssignment assignment, CancellationToken ct)
    {
        _context.TestAssignments.Update(assignment);
        await _context.SaveChangesAsync(ct);
    }
}
