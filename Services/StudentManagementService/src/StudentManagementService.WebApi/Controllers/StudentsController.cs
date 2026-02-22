using Microsoft.AspNetCore.Mvc;
using StudentManagementService.Application.Contracts;
using StudentManagementService.Application.DTOs.Requests;
using StudentManagementService.Application.DTOs.Responses;
using StudentManagementService.WebApi.Shared;

namespace StudentManagementService.WebApi.Controllers;

[ApiController]
[Route("api/v1/students")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;
    private readonly ITestAssignmentService _testAssignmentService;
    private readonly IProgressTrackingService _progressTrackingService;
    private readonly ILogger<StudentsController> _logger;

    public StudentsController(
        IStudentService studentService,
        ITestAssignmentService testAssignmentService,
        IProgressTrackingService progressTrackingService,
        ILogger<StudentsController> logger)
    {
        _studentService = studentService;
        _testAssignmentService = testAssignmentService;
        _progressTrackingService = progressTrackingService;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ResponseResultModel<StudentDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterStudent([FromBody] RegisterStudentRequest request, CancellationToken ct)
    {
        var result = await _studentService.RegisterStudentAsync(request, ct);

        var response = new ResponseResultModel<StudentDto>
        {
            IsError = false,
            Message = "Student registered successfully",
            Data = result
        };

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, response);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ResponseResultModel<StudentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _studentService.GetStudentByIdAsync(id, ct);

        var response = new ResponseResultModel<StudentDto>
        {
            IsError = false,
            Message = "Student retrieved successfully",
            Data = result
        };

        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ResponseResultModel<StudentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProfile(Guid id, [FromBody] UpdateStudentProfileRequest request, CancellationToken ct)
    {
        var result = await _studentService.UpdateStudentProfileAsync(id, request, ct);

        var response = new ResponseResultModel<StudentDto>
        {
            IsError = false,
            Message = "Student profile updated successfully",
            Data = result
        };

        return Ok(response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ResponseResultModel<StudentListDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStudents([FromQuery] Guid? groupId, [FromQuery] string? status, CancellationToken ct)
    {
        var result = await _studentService.GetStudentsAsync(groupId, status, ct);

        var response = new ResponseResultModel<StudentListDto>
        {
            IsError = false,
            Message = $"{result.TotalCount} students found",
            Data = result
        };

        return Ok(response);
    }

    [HttpPost("{studentId:guid}/tests/{testId:guid}/assign")]
    [ProducesResponseType(typeof(ResponseResultModel<TestAssignmentDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AssignTest(Guid studentId, Guid testId, [FromBody] AssignTestRequest? request, CancellationToken ct)
    {
        request ??= new AssignTestRequest();

        var result = await _testAssignmentService.AssignTestAsync(studentId, testId, request, ct);

        var response = new ResponseResultModel<TestAssignmentDto>
        {
            IsError = false,
            Message = "Test assigned to student",
            Data = result
        };

        return CreatedAtAction(nameof(GetStudentTests), new { id = studentId }, response);
    }

    [HttpGet("{id:guid}/tests")]
    [ProducesResponseType(typeof(ResponseResultModel<TestAssignmentListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStudentTests(Guid id, CancellationToken ct)
    {
        var result = await _testAssignmentService.GetStudentTestsAsync(id, ct);

        var response = new ResponseResultModel<TestAssignmentListDto>
        {
            IsError = false,
            Message = $"{result.TotalCount} tests retrieved",
            Data = result
        };

        return Ok(response);
    }

    [HttpGet("{id:guid}/tests/active")]
    [ProducesResponseType(typeof(ResponseResultModel<TestAssignmentListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetActiveTests(Guid id, CancellationToken ct)
    {
        var result = await _testAssignmentService.GetActiveTestsAsync(id, ct);

        var response = new ResponseResultModel<TestAssignmentListDto>
        {
            IsError = false,
            Message = $"{result.TotalCount} active test",
            Data = result
        };

        return Ok(response);
    }

    [HttpPost("{studentId:guid}/tests/{testId:guid}/submit")]
    [ProducesResponseType(typeof(ResponseResultModel<SubmitTestResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> SubmitTest(Guid studentId, Guid testId, [FromBody] SubmitTestRequest request, CancellationToken ct)
    {
        var result = await _testAssignmentService.SubmitTestAsync(studentId, testId, request, ct);

        var response = new ResponseResultModel<SubmitTestResponseDto>
        {
            IsError = false,
            Message = "Test submitted successfully",
            Data = result
        };

        return Ok(response);
    }

    [HttpGet("{id:guid}/progress")]
    [ProducesResponseType(typeof(ResponseResultModel<ProgressReportDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProgress(Guid id, CancellationToken ct)
    {
        var result = await _progressTrackingService.GetStudentProgressAsync(id, ct);

        var response = new ResponseResultModel<ProgressReportDto>
        {
            IsError = false,
            Message = "Progress report generated",
            Data = result
        };

        return Ok(response);
    }

    [HttpGet("{studentId:guid}/tests/{testId:guid}/results")]
    [ProducesResponseType(typeof(ResponseResultModel<TestResultDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTestResults(Guid studentId, Guid testId, CancellationToken ct)
    {
        var result = await _progressTrackingService.GetTestResultsAsync(studentId, testId, ct);

        var response = new ResponseResultModel<TestResultDto>
        {
            IsError = false,
            Message = "Test results retrieved",
            Data = result
        };

        return Ok(response);
    }
}
