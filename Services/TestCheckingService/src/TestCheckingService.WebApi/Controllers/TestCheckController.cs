using Microsoft.AspNetCore.Mvc;
using TestCheckingService.Application.Contracts;
using TestCheckingService.Application.DTOs.Requests;
using TestCheckingService.Application.DTOs.Responses;
using TestCheckingService.WebApi.Shared;

namespace TestCheckingService.WebApi.Controllers;

[ApiController]
[Route("api/v1/tests")]
public class TestCheckController : ControllerBase
{
    private readonly ITestCheckService _testCheckService;
    private readonly ILogger<TestCheckController> _logger;

    public TestCheckController(
        ITestCheckService testCheckService,
        ILogger<TestCheckController> logger)
    {
        _testCheckService = testCheckService;
        _logger = logger;
    }

    [HttpPost("{testId:guid}/check")]
    [ProducesResponseType(typeof(ResponseResultModel<TestResultDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> CheckTest(
        Guid testId,
        [FromBody] CheckTestRequest request,
        CancellationToken ct)
    {
        _logger.LogInformation("Checking test {TestId} for student {StudentId}", testId, request.StudentId);

        var result = await _testCheckService.CheckTestAsync(testId, request.StudentId, request.Answers, ct);

        var response = new ResponseResultModel<TestResultDto>
        {
            IsError = false,
            Message = "Test checked successfully",
            Data = result
        };

        return Ok(response);
    }

    [HttpPost("check/batch")]
    [ProducesResponseType(typeof(ResponseResultModel<BatchCheckResultDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> BatchCheck(
        [FromBody] BatchCheckRequest request,
        CancellationToken ct)
    {
        _logger.LogInformation("Processing batch check with {Count} items", request.Checks.Count);

        var result = await _testCheckService.BatchCheckAsync(request, ct);

        var response = new ResponseResultModel<BatchCheckResultDto>
        {
            IsError = false,
            Message = "Batch check completed",
            Data = result
        };

        return Ok(response);
    }
}
