namespace BffPortalService.Application.DTOs.Requests;

public record StartTestRequest
{
    public Guid TestId { get; init; }

    public Guid StudentId { get; init; }
}
