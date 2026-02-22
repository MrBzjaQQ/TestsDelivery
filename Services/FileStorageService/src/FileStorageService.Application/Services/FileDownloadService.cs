using FileStorageService.Application.Contracts;
using FileStorageService.Application.DTOs.Responses;
using FileStorageService.Application.Infrastructure.Database.Contract;
using Microsoft.Extensions.Logging;
using DomainExceptions = FileStorageService.Domain.Exceptions;

namespace FileStorageService.Application.Services;

public class FileDownloadService : IFileDownloadService
{
    private readonly IFileRepository _fileRepository;
    private readonly IImageProcessor _imageProcessor;
    private readonly ILogger<FileDownloadService> _logger;

    public FileDownloadService(
        IFileRepository fileRepository,
        IImageProcessor imageProcessor,
        ILogger<FileDownloadService> logger)
    {
        _fileRepository = fileRepository;
        _imageProcessor = imageProcessor;
        _logger = logger;
    }

    public async Task<FileDto> GetFileByIdAsync(Guid id, CancellationToken ct)
    {
        var file = await _fileRepository.GetByIdAsync(id, ct);

        if (file == null)
        {
            throw new DomainExceptions.FileNotFoundException(id);
        }

        return new FileDto(
            file.Id,
            file.FileName,
            file.ContentType,
            file.Size,
            file.CreatedAt,
            file.OwnerId,
            file.Width,
            file.Height,
            file.Data);
    }

    public async Task<FileDto> GetFileMetadataAsync(Guid id, CancellationToken ct)
    {
        var file = await _fileRepository.GetByIdAsync(id, ct);

        if (file == null)
        {
            throw new DomainExceptions.FileNotFoundException(id);
        }

        return new FileDto(
            file.Id,
            file.FileName,
            file.ContentType,
            file.Size,
            file.CreatedAt,
            file.OwnerId,
            file.Width,
            file.Height);
    }

    public async Task<byte[]> GetThumbnailAsync(Guid id, CancellationToken ct)
    {
        var file = await _fileRepository.GetByIdAsync(id, ct);

        if (file == null)
        {
            throw new DomainExceptions.FileNotFoundException(id);
        }

        if (!file.IsImage)
        {
            throw new DomainExceptions.InvalidFileException("File is not an image");
        }

        return await _imageProcessor.GenerateThumbnailAsync(file.Data, ct);
    }
}
