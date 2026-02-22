using Microsoft.AspNetCore.Mvc;
using TestCheckingService.Application.Contracts;
using TestCheckingService.Application.DTOs.Responses;
using TestCheckingService.WebApi.Shared;

namespace TestCheckingService.WebApi.Controllers;

[ApiController]
[Route("api/v1")]
public class ResultsController : ControllerBase
{
    private readonly IResultService _resultService;
    private readonly ILogger<ResultsController> _logger;

    public ResultsController(
        IResultService resultService,
        ILogger<ResultsController> logger)
    {
        _resultService = resultService;
        _logger = logger;
    }

    [HttpGet("tests/{testId:guid}/results")]
    [ProducesResponseType(typeof(ResponseResultModel<TestResultsListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTestResults(
        Guid testId,
        CancellationToken ct)
    {
        _logger.LogInformation("Getting results for test {TestId}", testId);

        var result = await _resultService.GetTestResultsAsync(testId, ct);

        var response = new ResponseResultModel<TestResultsListDto>
        {
            IsError = false,
            Message = "Results retrieved",
            Data = result
        };

        return Ok(response);
    }

    [HttpGet("tests/{testId:guid}/results/{studentId:guid}")]
    [ProducesResponseType(typeof(ResponseResultModel<DetailedTestResultDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTestResult(
        Guid testId,
        Guid studentId,
        CancellationToken ct)
    {
        _logger.LogInformation("Getting result for test {TestId} and student {StudentId}", testId, studentId);

        var result = await _resultService.GetDetailedResultAsync(testId, studentId, ct);

        var response = new ResponseResultModel<DetailedTestResultDto>
        {
            IsError = false,
            Message = "Result retrieved",
            Data = result
        };

        return Ok(response);
    }

    [HttpGet("students/{studentId:guid}/results")]
    [ProducesResponseType(typeof(ResponseResultModel<StudentResultsListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStudentResults(
        Guid studentId,
        CancellationToken ct)
    {
        _logger.LogInformation("Getting results for student {StudentId}", studentId);

        var result = await _resultService.GetStudentResultsAsync(studentId, ct);

        var response = new ResponseResultModel<StudentResultsListDto>
        {
            IsError = false,
            Message = "Student results retrieved",
            Data = result
        };

        return Ok(response);
    }
}
