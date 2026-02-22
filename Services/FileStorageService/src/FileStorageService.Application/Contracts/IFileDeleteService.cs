namespace FileStorageService.Application.Contracts;

public interface IFileDeleteService
{
    Task DeleteFileAsync(Guid id, CancellationToken ct);
}
