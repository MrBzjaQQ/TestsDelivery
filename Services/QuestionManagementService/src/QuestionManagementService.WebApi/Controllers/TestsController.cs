using QuestionManagementService.Application.Contracts;
using QuestionManagementService.Application.DTOs.Requests;
using QuestionManagementService.Application.DTOs.Responses;
using QuestionManagementService.WebApi.Shared;
using Microsoft.AspNetCore.Mvc;

namespace QuestionManagementService.WebApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TestsController : ControllerBase
{
    private readonly ITestService _testService;
    private readonly ILogger<TestsController> _logger;

    public TestsController(
        ITestService testService,
        ILogger<TestsController> logger)
    {
        _testService = testService;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ResponseResultModel<TestDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] CreateTestRequest request, CancellationToken ct)
    {
        var result = await _testService.CreateTestAsync(request, ct);

        var response = new ResponseResultModel<TestDto>
        {
            IsError = false,
            Message = "Test created successfully",
            Data = result
        };

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, response);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ResponseResultModel<TestDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _testService.GetTestByIdAsync(id, ct);

        var response = new ResponseResultModel<TestDto>
        {
            IsError = false,
            Message = "Test retrieved successfully",
            Data = result
        };

        return Ok(response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ResponseResultModel<TestListDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTests([FromQuery] Guid? templateId, CancellationToken ct)
    {
        if (!templateId.HasValue)
        {
            return BadRequest(new ResponseResultModel<TestListDto>
            {
                IsError = true,
                Message = "templateId query parameter is required"
            });
        }

        var result = await _testService.GetTestsByTemplateIdAsync(templateId.Value, ct);

        var response = new ResponseResultModel<TestListDto>
        {
            IsError = false,
            Message = $"{result.TotalCount} tests found",
            Data = result
        };

        return Ok(response);
    }

    [HttpPost("{id:guid}/copy")]
    [ProducesResponseType(typeof(ResponseResultModel<TestDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Copy(Guid id, CancellationToken ct)
    {
        var result = await _testService.CopyTestAsync(id, ct);

        var response = new ResponseResultModel<TestDto>
        {
            IsError = false,
            Message = "Test copied successfully",
            Data = result
        };

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, response);
    }

    [HttpPost("{id:guid}/generate")]
    [ProducesResponseType(typeof(ResponseResultModel<TestDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Generate(Guid id, [FromBody] GenerateTestRequest request, CancellationToken ct)
    {
        var result = await _testService.GenerateTestFromBankAsync(id, request, ct);

        var response = new ResponseResultModel<TestDto>
        {
            IsError = false,
            Message = "Test generated successfully",
            Data = result
        };

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, response);
    }
}
