using Microsoft.EntityFrameworkCore;
using StudentManagementService.Application.Infrastructure.Database.Contract;
using StudentManagementService.Domain.Entities;
using StudentManagementService.Infrastructure.Database.Context;

namespace StudentManagementService.Infrastructure.Database.Repositories;

public class StudyGroupRepository : IStudyGroupRepository
{
    private readonly IStudentDbContext _context;

    public StudyGroupRepository(IStudentDbContext context)
    {
        _context = context;
    }

    public async Task<StudyGroup?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _context.StudyGroups.FirstOrDefaultAsync(g => g.Id == id, ct);
    }

    public async Task AddAsync(StudyGroup group, CancellationToken ct)
    {
        await _context.StudyGroups.AddAsync(group, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<List<Student>> GetStudentsByGroupIdAsync(Guid groupId, CancellationToken ct)
    {
        return await _context.Students.Where(s => s.GroupId == groupId).ToListAsync(ct);
    }
}
