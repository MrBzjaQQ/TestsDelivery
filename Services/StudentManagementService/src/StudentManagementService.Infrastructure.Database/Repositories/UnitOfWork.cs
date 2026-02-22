using StudentManagementService.Application.Infrastructure.Database.Contract;
using StudentManagementService.Infrastructure.Database.Context;

namespace StudentManagementService.Infrastructure.Database.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly IStudentDbContext _context;

    public UnitOfWork(IStudentDbContext context)
    {
        _context = context;
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct)
    {
        return await _context.SaveChangesAsync(ct);
    }
}
