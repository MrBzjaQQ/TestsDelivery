namespace FileStorageService.Application.DTOs.Requests;

public record UploadFileRequest(
    string FileName,
    string ContentType,
    long Length,
    Func<Stream, CancellationToken, Task<byte[]>> GetBytesAsync);
