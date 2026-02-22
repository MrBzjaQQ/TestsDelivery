using TestCheckingService.Application.Infrastructure.Database.Contract;
using TestCheckingService.Infrastructure.Database.Context;

namespace TestCheckingService.Infrastructure.Database.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ITestCheckingDbContext _context;

    public UnitOfWork(ITestCheckingDbContext context)
    {
        _context = context;
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct)
    {
        return await _context.SaveChangesAsync(ct);
    }
}
