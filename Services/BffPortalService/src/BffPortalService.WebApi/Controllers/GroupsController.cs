using BffPortalService.Application.Contracts;
using BffPortalService.Application.DTOs.Responses;
using BffPortalService.WebApi.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BffPortalService.WebApi.Controllers;

[ApiController]
[Route("api/v1/portal/groups")]
[Authorize]
public class GroupsController : ControllerBase
{
    private readonly IGroupAnalyticsService _groupAnalyticsService;
    private readonly ILogger<GroupsController> _logger;

    public GroupsController(
        IGroupAnalyticsService groupAnalyticsService,
        ILogger<GroupsController> logger)
    {
        _groupAnalyticsService = groupAnalyticsService;
        _logger = logger;
    }

    [HttpGet("{id:guid}/analytics")]
    [ProducesResponseType(typeof(ResponseResultModel<GroupAnalyticsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetGroupAnalytics(Guid id, CancellationToken ct)
    {
        var accessToken = GetAccessToken();

        var analytics = await _groupAnalyticsService.GetGroupAnalyticsAsync(id, accessToken, ct);

        var response = new ResponseResultModel<GroupAnalyticsDto>
        {
            IsError = false,
            Message = "Group analytics retrieved",
            Data = analytics
        };

        return Ok(response);
    }

    private string GetAccessToken()
    {
        var authorizationHeader = HttpContext.Request.Headers.Authorization.ToString();
        return authorizationHeader.Replace("Bearer ", string.Empty);
    }
}
