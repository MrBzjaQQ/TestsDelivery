namespace FileStorageService.Domain.Exceptions;

public class InvalidFileException : Exception
{
    public List<FileValidationViolation> Violations { get; }

    public InvalidFileException(string message, List<FileValidationViolation>? violations = null)
        : base(message)
    {
        Violations = violations ?? [];
    }
}

public record FileValidationViolation(string Field, string Message, string Code);
