using QuestionManagementService.Application.Contracts;
using QuestionManagementService.Application.DTOs.Requests;
using QuestionManagementService.Application.DTOs.Responses;
using QuestionManagementService.WebApi.Shared;
using Microsoft.AspNetCore.Mvc;

namespace QuestionManagementService.WebApi.Controllers;

[ApiController]
[Route("api/v1/question-banks")]
public class QuestionBanksController : ControllerBase
{
    private readonly IQuestionBankService _questionBankService;
    private readonly ILogger<QuestionBanksController> _logger;

    public QuestionBanksController(
        IQuestionBankService questionBankService,
        ILogger<QuestionBanksController> logger)
    {
        _questionBankService = questionBankService;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ResponseResultModel<QuestionBankDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateQuestionBankRequest request, CancellationToken ct)
    {
        var result = await _questionBankService.CreateQuestionBankAsync(request, ct);

        var response = new ResponseResultModel<QuestionBankDto>
        {
            IsError = false,
            Message = "Question bank created successfully",
            Data = result
        };

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, response);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ResponseResultModel<QuestionBankDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _questionBankService.GetQuestionBankByIdAsync(id, ct);

        var response = new ResponseResultModel<QuestionBankDto>
        {
            IsError = false,
            Message = "Question bank retrieved successfully",
            Data = result
        };

        return Ok(response);
    }

    [HttpGet("{id:guid}/questions")]
    [ProducesResponseType(typeof(ResponseResultModel<QuestionListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetQuestions(Guid id, CancellationToken ct)
    {
        var result = await _questionBankService.GetQuestionsByBankIdAsync(id, ct);

        var response = new ResponseResultModel<QuestionListDto>
        {
            IsError = false,
            Message = $"{result.TotalCount} questions retrieved",
            Data = result
        };

        return Ok(response);
    }
}
