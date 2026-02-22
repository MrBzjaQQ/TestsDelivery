using FileStorageService.Application.DTOs.Responses;

namespace FileStorageService.Application.Contracts;

public interface IFileDownloadService
{
    Task<FileDto> GetFileByIdAsync(Guid id, CancellationToken ct);

    Task<FileDto> GetFileMetadataAsync(Guid id, CancellationToken ct);

    Task<byte[]> GetThumbnailAsync(Guid id, CancellationToken ct);
}
