using FileStorageService.Application.Contracts;
using FileStorageService.Application.DTOs.Requests;
using FileStorageService.Application.DTOs.Responses;
using FileStorageService.WebApi.Shared;
using Microsoft.AspNetCore.Mvc;

namespace FileStorageService.WebApi.Controllers;

[ApiController]
[Route("api/v1/files")]
public class FilesController : ControllerBase
{
    private readonly IFileUploadService _uploadService;
    private readonly IFileDownloadService _downloadService;
    private readonly IFileDeleteService _deleteService;
    private readonly ILogger<FilesController> _logger;

    public FilesController(
        IFileUploadService uploadService,
        IFileDownloadService downloadService,
        IFileDeleteService deleteService,
        ILogger<FilesController> logger)
    {
        _uploadService = uploadService;
        _downloadService = downloadService;
        _deleteService = deleteService;
        _logger = logger;
    }

    [HttpPost("upload")]
    [ProducesResponseType(typeof(ResponseResultModel<UploadResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status413PayloadTooLarge)]
    public async Task<IActionResult> Upload(IFormFile file, [FromForm] Guid ownerId, CancellationToken ct)
    {
        var request = new UploadFileRequest(
            file.FileName,
            file.ContentType,
            file.Length,
            async (stream, cancellationToken) =>
            {
                await file.CopyToAsync(stream, cancellationToken);
                return ((MemoryStream)stream).ToArray();
            });

        var result = await _uploadService.UploadFileAsync(request, ownerId, ct);

        var response = new ResponseResultModel<UploadResponseDto>
        {
            IsError = false,
            Message = "File uploaded successfully",
            Data = result
        };

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, response);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(File), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var file = await _downloadService.GetFileByIdAsync(id, ct);

        return File(file.Data!, file.ContentType, file.FileName, enableRangeProcessing: true);
    }

    [HttpGet("{id:guid}/metadata")]
    [ProducesResponseType(typeof(ResponseResultModel<FileDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMetadata(Guid id, CancellationToken ct)
    {
        var file = await _downloadService.GetFileMetadataAsync(id, ct);

        var response = new ResponseResultModel<FileDto>
        {
            IsError = false,
            Message = "File metadata retrieved",
            Data = file
        };

        return Ok(response);
    }

    [HttpGet("{id:guid}/thumbnail")]
    [ProducesResponseType(typeof(File), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetThumbnail(Guid id, CancellationToken ct)
    {
        var thumbnail = await _downloadService.GetThumbnailAsync(id, ct);

        return File(thumbnail, "image/jpeg", $"thumbnail_{id}.jpg");
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _deleteService.DeleteFileAsync(id, ct);
        return NoContent();
    }
}
