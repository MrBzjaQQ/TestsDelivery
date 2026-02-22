using Microsoft.AspNetCore.Mvc;
using StudentManagementService.Application.Contracts;
using StudentManagementService.Application.DTOs.Responses;
using StudentManagementService.WebApi.Shared;

namespace StudentManagementService.WebApi.Controllers;

[ApiController]
[Route("api/v1/groups")]
public class GroupsController : ControllerBase
{
    private readonly IGroupService _groupService;
    private readonly ILogger<GroupsController> _logger;

    public GroupsController(IGroupService groupService, ILogger<GroupsController> logger)
    {
        _groupService = groupService;
        _logger = logger;
    }

    [HttpGet("{id:guid}/statistics")]
    [ProducesResponseType(typeof(ResponseResultModel<GroupStatisticsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStatistics(Guid id, CancellationToken ct)
    {
        var result = await _groupService.GetGroupStatisticsAsync(id, ct);

        var response = new ResponseResultModel<GroupStatisticsDto>
        {
            IsError = false,
            Message = "Group statistics retrieved",
            Data = result
        };

        return Ok(response);
    }

    [HttpGet("{id:guid}/students")]
    [ProducesResponseType(typeof(ResponseResultModel<StudentListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStudents(Guid id, CancellationToken ct)
    {
        var result = await _groupService.GetGroupStudentsAsync(id, ct);

        var response = new ResponseResultModel<StudentListDto>
        {
            IsError = false,
            Message = $"{result.TotalCount} students in group",
            Data = result
        };

        return Ok(response);
    }
}
