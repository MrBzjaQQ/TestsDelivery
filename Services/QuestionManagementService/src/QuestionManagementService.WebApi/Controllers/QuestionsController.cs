using QuestionManagementService.Application.Contracts;
using QuestionManagementService.Application.DTOs.Requests;
using QuestionManagementService.Application.DTOs.Responses;
using QuestionManagementService.WebApi.Shared;
using Microsoft.AspNetCore.Mvc;

namespace QuestionManagementService.WebApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class QuestionsController : ControllerBase
{
    private readonly IQuestionService _questionService;
    private readonly ILogger<QuestionsController> _logger;

    public QuestionsController(
        IQuestionService questionService,
        ILogger<QuestionsController> logger)
    {
        _questionService = questionService;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ResponseResultModel<QuestionDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] CreateQuestionRequest request, CancellationToken ct)
    {
        var result = await _questionService.CreateQuestionAsync(request, ct);

        var response = new ResponseResultModel<QuestionDto>
        {
            IsError = false,
            Message = "Question created successfully",
            Data = result
        };

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, response);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ResponseResultModel<QuestionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _questionService.GetQuestionByIdAsync(id, ct);

        var response = new ResponseResultModel<QuestionDto>
        {
            IsError = false,
            Message = "Question retrieved successfully",
            Data = result
        };

        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ResponseResultModel<QuestionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateQuestionRequest request, CancellationToken ct)
    {
        var result = await _questionService.UpdateQuestionAsync(id, request, ct);

        var response = new ResponseResultModel<QuestionDto>
        {
            IsError = false,
            Message = "Question updated successfully",
            Data = result
        };

        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _questionService.DeleteQuestionAsync(id, ct);
        return NoContent();
    }

    [HttpGet]
    [ProducesResponseType(typeof(ResponseResultModel<QuestionListDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetQuestions(
        [FromQuery] string? category,
        [FromQuery] string? difficulty,
        [FromQuery] Guid? questionBankId,
        CancellationToken ct)
    {
        var result = await _questionService.GetQuestionsAsync(category, difficulty, questionBankId, ct);

        var response = new ResponseResultModel<QuestionListDto>
        {
            IsError = false,
            Message = $"{result.TotalCount} questions found",
            Data = result
        };

        return Ok(response);
    }
}
