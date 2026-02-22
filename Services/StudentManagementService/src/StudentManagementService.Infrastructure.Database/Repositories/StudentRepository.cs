using Microsoft.EntityFrameworkCore;
using StudentManagementService.Application.Infrastructure.Database.Contract;
using StudentManagementService.Domain.Entities;
using StudentManagementService.Infrastructure.Database.Context;

namespace StudentManagementService.Infrastructure.Database.Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly IStudentDbContext _context;

    public StudentRepository(IStudentDbContext context)
    {
        _context = context;
    }

    public async Task<Student?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _context.Students.FirstOrDefaultAsync(s => s.Id == id, ct);
    }

    public async Task<Student?> GetByUserIdAsync(Guid userId, CancellationToken ct)
    {
        return await _context.Students.FirstOrDefaultAsync(s => s.UserId == userId, ct);
    }

    public async Task<List<Student>> GetByGroupIdAsync(Guid groupId, CancellationToken ct)
    {
        return await _context.Students.Where(s => s.GroupId == groupId).ToListAsync(ct);
    }

    public async Task<List<Student>> GetByStatusAsync(StudentStatus status, CancellationToken ct)
    {
        return await _context.Students.Where(s => s.Status == status).ToListAsync(ct);
    }

    public async Task<List<Student>> GetByGroupIdAndStatusAsync(Guid groupId, StudentStatus status, CancellationToken ct)
    {
        return await _context.Students
            .Where(s => s.GroupId == groupId && s.Status == status)
            .ToListAsync(ct);
    }

    public async Task AddAsync(Student student, CancellationToken ct)
    {
        await _context.Students.AddAsync(student, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Student student, CancellationToken ct)
    {
        _context.Students.Update(student);
        await _context.SaveChangesAsync(ct);
    }
}
