using IdentityService.Application.Contracts;
using IdentityService.Application.DTOs.Requests;
using IdentityService.Application.DTOs.Responses;
using IdentityService.WebApi.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IdentityService.WebApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IUserService userService, ILogger<UsersController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseResultModel<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetUser(string id, CancellationToken cancellationToken)
    {
        var result = await _userService.GetUserByIdAsync(id, cancellationToken);

        var response = new ResponseResultModel<UserDto>
        {
            IsError = false,
            Message = "User retrieved",
            Data = result,
        };

        return Ok(response);
    }

    [HttpPost("{id}/roles")]
    [ProducesResponseType(typeof(ResponseResultModel<RoleAssignmentResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AssignRole(string id, [FromBody] AssignRoleRequest request, CancellationToken cancellationToken)
    {
        var result = await _userService.AssignRoleAsync(id, request.Role, cancellationToken);

        _logger.LogInformation("Role {Role} assigned to user {UserId}", request.Role, id);

        var response = new ResponseResultModel<RoleAssignmentResponse>
        {
            IsError = false,
            Message = "Role assigned successfully",
            Data = result,
        };

        return Ok(response);
    }
}
