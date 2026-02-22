namespace FileStorageService.Application.Settings;

public sealed record FileStorageSettings
{
    public int MaxFileSizeMB { get; init; } = 10;
    public List<string> AllowedImageTypes { get; init; } = [];
    public List<string> AllowedDocumentTypes { get; init; } = [];
    public int ThumbnailWidth { get; init; } = 200;
    public int ThumbnailHeight { get; init; } = 200;
    public int MaxImageWidth { get; init; } = 1920;
    public int MaxImageHeight { get; init; } = 1080;
    public int JpegQuality { get; init; } = 85;
    public int ThumbnailQuality { get; init; } = 75;
}
