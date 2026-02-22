using FileStorageService.Application.DTOs.Requests;
using FileStorageService.Application.DTOs.Responses;

namespace FileStorageService.Application.Contracts;

public interface IFileValidator
{
    FileValidationResult Validate(UploadFileRequest file);
}
