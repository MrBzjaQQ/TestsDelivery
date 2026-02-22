namespace FileStorageService.WebApi.Settings;

public sealed record AppSettings
{
    public required string ConnectionString { get; init; }
    public FileStorageSettings FileStorage { get; init; } = new();
}

public sealed record FileStorageSettings
{
    public int MaxFileSizeMB { get; init; } = 10;
    public List<string> AllowedImageTypes { get; init; } = ["image/jpeg", "image/png", "image/gif", "image/webp"];
    public List<string> AllowedDocumentTypes { get; init; } = ["application/pdf", "application/msword", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"];
    public int ThumbnailWidth { get; init; } = 200;
    public int ThumbnailHeight { get; init; } = 200;
    public int MaxImageWidth { get; init; } = 1920;
    public int MaxImageHeight { get; init; } = 1080;
    public int JpegQuality { get; init; } = 85;
    public int ThumbnailQuality { get; init; } = 75;
}
