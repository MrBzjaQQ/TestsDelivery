using Microsoft.EntityFrameworkCore;
using DomainEntities = FileStorageService.Domain.Entities;

namespace FileStorageService.Infrastructure.Database.Context;

public interface IFileStorageDbContext
{
    DbSet<DomainEntities.File> Files { get; }

    Task<int> SaveChangesAsync(CancellationToken ct);
}
