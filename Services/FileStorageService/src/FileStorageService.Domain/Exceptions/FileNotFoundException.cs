namespace FileStorageService.Domain.Exceptions;

public class FileNotFoundException : Exception
{
    public Guid FileId { get; }

    public FileNotFoundException(Guid fileId)
        : base($"File with id '{fileId}' not found")
    {
        FileId = fileId;
    }
}
