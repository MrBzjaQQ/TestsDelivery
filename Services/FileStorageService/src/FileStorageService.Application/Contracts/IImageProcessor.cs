namespace FileStorageService.Application.Contracts;

public interface IImageProcessor
{
    Task<byte[]> ProcessAsync(byte[] imageBytes, CancellationToken ct = default);

    Task<byte[]> GenerateThumbnailAsync(byte[] imageBytes, CancellationToken ct = default);
}
