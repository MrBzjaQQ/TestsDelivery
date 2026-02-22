namespace FileStorageService.Application.Infrastructure.Database.Contract;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken ct);
}
