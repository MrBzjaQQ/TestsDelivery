using BffPortalService.Application.DTOs.Responses;

namespace BffPortalService.Application.Contracts;

public interface IGroupAnalyticsService
{
    Task<GroupAnalyticsDto> GetGroupAnalyticsAsync(Guid groupId, string accessToken, CancellationToken ct);

    Task InvalidateGroupCacheAsync(Guid groupId, CancellationToken ct);
}
