using FileStorageService.Application.Infrastructure.Database.Contract;
using FileStorageService.Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;
using DomainEntities = FileStorageService.Domain.Entities;

namespace FileStorageService.Infrastructure.Database.Repositories;

public class FileRepository : IFileRepository
{
    private readonly IFileStorageDbContext _context;

    public FileRepository(IFileStorageDbContext context)
    {
        _context = context;
    }

    public async Task<DomainEntities.File?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _context.Files.FirstOrDefaultAsync(f => f.Id == id, ct);
    }

    public async Task AddAsync(DomainEntities.File file, CancellationToken ct)
    {
        await _context.Files.AddAsync(file, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct)
    {
        var file = await _context.Files.FirstOrDefaultAsync(f => f.Id == id, ct);
        if (file != null)
        {
            _context.Files.Remove(file);
            await _context.SaveChangesAsync(ct);
        }
    }
}
