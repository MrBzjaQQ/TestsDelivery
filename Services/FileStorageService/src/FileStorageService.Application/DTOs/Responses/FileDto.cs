namespace FileStorageService.Application.DTOs.Responses;

public record FileDto(
    Guid Id,
    string FileName,
    string ContentType,
    long Size,
    DateTime UploadedAt,
    Guid OwnerId,
    int? Width,
    int? Height,
    byte[]? Data = null);
