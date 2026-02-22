using FileStorageService.Application.Contracts;
using FileStorageService.Application.DTOs.Requests;
using FileStorageService.Application.DTOs.Responses;
using FileStorageService.Application.Infrastructure.Database.Contract;
using Microsoft.Extensions.Logging;
using DomainEntities = FileStorageService.Domain.Entities;
using DomainExceptions = FileStorageService.Domain.Exceptions;

namespace FileStorageService.Application.Services;

public class FileUploadService : IFileUploadService
{
    private readonly IFileRepository _fileRepository;
    private readonly IImageProcessor _imageProcessor;
    private readonly IFileValidator _fileValidator;
    private readonly ILogger<FileUploadService> _logger;

    public FileUploadService(
        IFileRepository fileRepository,
        IImageProcessor imageProcessor,
        IFileValidator fileValidator,
        ILogger<FileUploadService> logger)
    {
        _fileRepository = fileRepository;
        _imageProcessor = imageProcessor;
        _fileValidator = fileValidator;
        _logger = logger;
    }

    public async Task<UploadResponseDto> UploadFileAsync(UploadFileRequest request, Guid ownerId, CancellationToken ct)
    {
        var validation = _fileValidator.Validate(request);
        if (!validation.IsValid)
        {
            throw new DomainExceptions.InvalidFileException(validation.ErrorMessage!);
        }

        using var memoryStream = new MemoryStream();
        var fileBytes = await request.GetBytesAsync(memoryStream, ct);

        var isImage = request.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase);
        int? width = null;
        int? height = null;
        byte[] processedBytes = fileBytes;

        if (isImage)
        {
            processedBytes = await _imageProcessor.ProcessAsync(fileBytes, ct);
            using var img = SixLabors.ImageSharp.Image.Load(processedBytes);
            width = img.Width;
            height = img.Height;
        }

        var fileEntity = new DomainEntities.File
        {
            Id = Guid.NewGuid(),
            FileName = request.FileName,
            ContentType = request.ContentType,
            Size = processedBytes.Length,
            Data = processedBytes,
            OwnerId = ownerId,
            Width = width,
            Height = height,
            IsImage = isImage,
            CreatedAt = DateTime.UtcNow
        };

        await _fileRepository.AddAsync(fileEntity, ct);

        _logger.LogInformation("File uploaded: {FileName}, Size: {Size} bytes", fileEntity.FileName, fileEntity.Size);

        return new UploadResponseDto(
            fileEntity.Id,
            fileEntity.FileName,
            fileEntity.ContentType,
            fileEntity.Size,
            fileEntity.CreatedAt,
            fileEntity.OwnerId);
    }
}
