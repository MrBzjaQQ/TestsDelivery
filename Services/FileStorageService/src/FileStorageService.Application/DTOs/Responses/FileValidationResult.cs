namespace FileStorageService.Application.Contracts;

public record FileValidationResult(bool IsValid, string? ErrorMessage);
