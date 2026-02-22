using File = FileStorageService.Domain.Entities.File;

namespace FileStorageService.Application.Infrastructure.Database.Contract;

public interface IFileRepository
{
    Task<File?> GetByIdAsync(Guid id, CancellationToken ct);

    Task AddAsync(File file, CancellationToken ct);

    Task DeleteAsync(Guid id, CancellationToken ct);
}
