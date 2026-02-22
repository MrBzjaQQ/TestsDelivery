using QuestionManagementService.Application.Contracts;
using QuestionManagementService.Application.DTOs.Requests;
using QuestionManagementService.Application.DTOs.Responses;
using QuestionManagementService.WebApi.Shared;
using Microsoft.AspNetCore.Mvc;

namespace QuestionManagementService.WebApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TemplatesController : ControllerBase
{
    private readonly ITestTemplateService _templateService;
    private readonly ILogger<TemplatesController> _logger;

    public TemplatesController(
        ITestTemplateService templateService,
        ILogger<TemplatesController> logger)
    {
        _templateService = templateService;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ResponseResultModel<TestTemplateDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateTestTemplateRequest request, CancellationToken ct)
    {
        var result = await _templateService.CreateTemplateAsync(request, ct);

        var response = new ResponseResultModel<TestTemplateDto>
        {
            IsError = false,
            Message = "Template created successfully",
            Data = result
        };

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, response);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ResponseResultModel<TestTemplateDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _templateService.GetTemplateByIdAsync(id, ct);

        var response = new ResponseResultModel<TestTemplateDto>
        {
            IsError = false,
            Message = "Template retrieved successfully",
            Data = result
        };

        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ResponseResultModel<TestTemplateDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] CreateTestTemplateRequest request, CancellationToken ct)
    {
        var result = await _templateService.UpdateTemplateAsync(id, request, ct);

        var response = new ResponseResultModel<TestTemplateDto>
        {
            IsError = false,
            Message = "Template updated successfully",
            Data = result
        };

        return Ok(response);
    }
}
