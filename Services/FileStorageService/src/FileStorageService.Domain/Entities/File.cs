namespace FileStorageService.Domain.Entities;

public class File
{
    public Guid Id { get; init; }

    public string FileName { get; init; } = string.Empty;

    public string ContentType { get; init; } = string.Empty;

    public long Size { get; init; }

    public byte[] Data { get; init; } = [];

    public Guid OwnerId { get; init; }

    public int? Width { get; init; }

    public int? Height { get; init; }

    public bool IsImage { get; init; }

    public DateTime CreatedAt { get; init; }
}
