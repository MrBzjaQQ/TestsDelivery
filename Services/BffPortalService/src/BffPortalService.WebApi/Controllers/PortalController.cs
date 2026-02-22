using System.Security.Claims;
using BffPortalService.Application.Contracts;
using BffPortalService.Application.DTOs.Responses;
using BffPortalService.WebApi.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BffPortalService.WebApi.Controllers;

[ApiController]
[Route("api/v1/portal")]
[Authorize]
public class PortalController : ControllerBase
{
    private readonly IStudentPortalService _studentPortalService;
    private readonly ILogger<PortalController> _logger;

    public PortalController(
        IStudentPortalService studentPortalService,
        ILogger<PortalController> logger)
    {
        _studentPortalService = studentPortalService;
        _logger = logger;
    }

    [HttpGet("students/profile")]
    [ProducesResponseType(typeof(ResponseResultModel<StudentProfileDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStudentProfile(CancellationToken ct)
    {
        var accessToken = GetAccessToken();
        var userId = GetUserId();

        var profile = await _studentPortalService.GetStudentProfileAsync(userId, accessToken, ct);

        var response = new ResponseResultModel<StudentProfileDto>
        {
            IsError = false,
            Message = "Student profile retrieved",
            Data = profile
        };

        return Ok(response);
    }

    private string GetAccessToken()
    {
        var authorizationHeader = HttpContext.Request.Headers.Authorization.ToString();
        return authorizationHeader.Replace("Bearer ", string.Empty);
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? User.FindFirst("sub")?.Value;

        return Guid.TryParse(userIdClaim, out var userId)
            ? userId
            : throw new UnauthorizedAccessException("Invalid user identifier");
    }
}
