using FileStorageService.Application.Contracts;
using FileStorageService.Application.Settings;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace FileStorageService.Application.Services;

public class ImageProcessor : IImageProcessor
{
    private readonly FileStorageSettings _settings;

    public ImageProcessor(IOptions<FileStorageSettings> settings)
    {
        _settings = settings.Value;
    }

    public async Task<byte[]> ProcessAsync(byte[] imageBytes, CancellationToken ct = default)
    {
        using var image = Image.Load(imageBytes);

        if (image.Width > _settings.MaxImageWidth || image.Height > _settings.MaxImageHeight)
        {
            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Mode = ResizeMode.Max,
                Size = new Size(_settings.MaxImageWidth, _settings.MaxImageHeight)
            }));
        }

        using var memoryStream = new MemoryStream();
        await image.SaveAsync(memoryStream, new JpegEncoder { Quality = _settings.JpegQuality }, ct);
        return memoryStream.ToArray();
    }

    public async Task<byte[]> GenerateThumbnailAsync(byte[] imageBytes, CancellationToken ct = default)
    {
        using var image = Image.Load(imageBytes);

        image.Mutate(x => x.Resize(new ResizeOptions
        {
            Mode = ResizeMode.Crop,
            Size = new Size(_settings.ThumbnailWidth, _settings.ThumbnailHeight)
        }));

        using var memoryStream = new MemoryStream();
        await image.SaveAsync(memoryStream, new JpegEncoder { Quality = _settings.ThumbnailQuality }, ct);
        return memoryStream.ToArray();
    }
}
