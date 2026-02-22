using System.Security.Claims;
using BffPortalService.Application.Contracts;
using BffPortalService.Application.DTOs.Requests;
using BffPortalService.Application.DTOs.Responses;
using BffPortalService.WebApi.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BffPortalService.WebApi.Controllers;

[ApiController]
[Route("api/v1/portal/tests")]
[Authorize]
public class TestsController : ControllerBase
{
    private readonly ITestsPortalService _testsPortalService;
    private readonly ILogger<TestsController> _logger;

    public TestsController(
        ITestsPortalService testsPortalService,
        ILogger<TestsController> logger)
    {
        _testsPortalService = testsPortalService;
        _logger = logger;
    }

    [HttpGet("available")]
    [ProducesResponseType(typeof(ResponseResultModel<AvailableTestsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAvailableTests(
        [FromQuery] Guid? groupId,
        [FromQuery] string? status,
        CancellationToken ct)
    {
        var accessToken = GetAccessToken();
        var userId = GetUserId();

        var request = new GetTestsRequest
        {
            GroupId = groupId,
            Status = status
        };

        var tests = await _testsPortalService.GetAvailableTestsAsync(userId, accessToken, request, ct);

        var response = new ResponseResultModel<AvailableTestsDto>
        {
            IsError = false,
            Message = $"{tests.TotalCount} tests found",
            Data = tests
        };

        return Ok(response);
    }

    [HttpPost("{id:guid}/start")]
    [ProducesResponseType(typeof(ResponseResultModel<StartTestResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> StartTest(Guid id, CancellationToken ct)
    {
        var accessToken = GetAccessToken();
        var studentId = GetUserId();

        var result = await _testsPortalService.StartTestAsync(id, studentId, accessToken, ct);

        var response = new ResponseResultModel<StartTestResponseDto>
        {
            IsError = false,
            Message = "Test started",
            Data = result
        };

        return Ok(response);
    }

    [HttpGet("{testId:guid}/questions")]
    [ProducesResponseType(typeof(ResponseResultModel<TestQuestionsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTestQuestions(Guid testId, CancellationToken ct)
    {
        var accessToken = GetAccessToken();

        var questions = await _testsPortalService.GetTestQuestionsAsync(testId, accessToken, ct);

        var response = new ResponseResultModel<TestQuestionsDto>
        {
            IsError = false,
            Message = "Test questions retrieved",
            Data = questions
        };

        return Ok(response);
    }

    [HttpPost("{testId:guid}/submit")]
    [ProducesResponseType(typeof(ResponseResultModel<SubmitTestResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SubmitTest(Guid testId, [FromBody] SubmitTestRequestDto request, CancellationToken ct)
    {
        var accessToken = GetAccessToken();
        var studentId = GetUserId();

        var submitRequest = new SubmitTestRequest
        {
            TestId = testId,
            StudentId = studentId,
            Answers = request.Answers.Select(a => new AnswerDto
            {
                QuestionId = a.QuestionId,
                SelectedOptionIds = a.SelectedOptionIds,
                TextAnswer = a.TextAnswer
            }).ToList()
        };

        var result = await _testsPortalService.SubmitTestAnswersAsync(testId, studentId, accessToken, submitRequest, ct);

        var response = new ResponseResultModel<SubmitTestResponseDto>
        {
            IsError = false,
            Message = "Test submitted successfully",
            Data = result
        };

        return Ok(response);
    }

    [HttpGet("{testId:guid}/results")]
    [ProducesResponseType(typeof(ResponseResultModel<TestResultsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTestResults(Guid testId, CancellationToken ct)
    {
        var accessToken = GetAccessToken();
        var studentId = GetUserId();

        var results = await _testsPortalService.GetTestResultsAsync(testId, studentId, accessToken, ct);

        var response = new ResponseResultModel<TestResultsDto>
        {
            IsError = false,
            Message = "Test results retrieved",
            Data = results
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

public record SubmitTestRequestDto
{
    public List<AnswerItemDto> Answers { get; init; } = [];
}

public record AnswerItemDto
{
    public Guid QuestionId { get; init; }
    public List<Guid> SelectedOptionIds { get; init; } = [];
    public string? TextAnswer { get; init; }
}
