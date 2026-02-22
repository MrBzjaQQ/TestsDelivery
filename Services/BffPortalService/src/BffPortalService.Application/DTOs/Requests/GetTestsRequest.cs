namespace BffPortalService.Application.DTOs.Requests;

public record GetTestsRequest
{
    public Guid? GroupId { get; init; }

    public string? Status { get; init; }
}
