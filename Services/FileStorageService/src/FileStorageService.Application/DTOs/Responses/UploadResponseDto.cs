namespace FileStorageService.Application.DTOs.Responses;

public record UploadResponseDto(
    Guid Id,
    string FileName,
    string ContentType,
    long Size,
    DateTime UploadedAt,
    Guid OwnerId);
