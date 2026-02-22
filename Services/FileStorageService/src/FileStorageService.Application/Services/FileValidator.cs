using FileStorageService.Application.Contracts;
using FileStorageService.Application.DTOs.Requests;
using FileStorageService.Application.Settings;
using Microsoft.Extensions.Options;

namespace FileStorageService.Application.Services;

public class FileValidator : IFileValidator
{
    private readonly FileStorageSettings _settings;

    public FileValidator(IOptions<FileStorageSettings> settings)
    {
        _settings = settings.Value;
    }

    public FileValidationResult Validate(UploadFileRequest file)
    {
        var allowedTypes = _settings.AllowedImageTypes.Concat(_settings.AllowedDocumentTypes).ToList();

        if (file.Length > _settings.MaxFileSizeMB * 1024 * 1024)
        {
            return new FileValidationResult(false, $"Maximum file size is {_settings.MaxFileSizeMB}MB");
        }

        if (!allowedTypes.Contains(file.ContentType))
        {
            return new FileValidationResult(false, $"File type not allowed: {file.ContentType}");
        }

        return new FileValidationResult(true, null);
    }
}
