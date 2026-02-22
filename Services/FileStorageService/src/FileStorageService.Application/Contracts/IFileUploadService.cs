using FileStorageService.Application.DTOs.Requests;
using FileStorageService.Application.DTOs.Responses;

namespace FileStorageService.Application.Contracts;

public interface IFileUploadService
{
    Task<UploadResponseDto> UploadFileAsync(UploadFileRequest request, Guid ownerId, CancellationToken ct);
}
