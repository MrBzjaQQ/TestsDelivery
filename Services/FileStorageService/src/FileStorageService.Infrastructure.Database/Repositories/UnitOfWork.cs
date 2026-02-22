using FileStorageService.Application.Infrastructure.Database.Contract;
using FileStorageService.Infrastructure.Database.Context;

namespace FileStorageService.Infrastructure.Database.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly IFileStorageDbContext _context;

    public UnitOfWork(IFileStorageDbContext context)
    {
        _context = context;
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct)
    {
        return await _context.SaveChangesAsync(ct);
    }
}
