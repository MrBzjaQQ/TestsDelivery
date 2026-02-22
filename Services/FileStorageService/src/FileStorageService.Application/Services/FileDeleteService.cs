using FileStorageService.Application.Contracts;
using FileStorageService.Application.Infrastructure.Database.Contract;
using Microsoft.Extensions.Logging;
using DomainExceptions = FileStorageService.Domain.Exceptions;

namespace FileStorageService.Application.Services;

public class FileDeleteService : IFileDeleteService
{
    private readonly IFileRepository _fileRepository;
    private readonly ILogger<FileDeleteService> _logger;

    public FileDeleteService(
        IFileRepository fileRepository,
        ILogger<FileDeleteService> logger)
    {
        _fileRepository = fileRepository;
        _logger = logger;
    }

    public async Task DeleteFileAsync(Guid id, CancellationToken ct)
    {
        var file = await _fileRepository.GetByIdAsync(id, ct);

        if (file == null)
        {
            throw new DomainExceptions.FileNotFoundException(id);
        }

        await _fileRepository.DeleteAsync(id, ct);
        _logger.LogInformation("File deleted: {FileId}", id);
    }
}
