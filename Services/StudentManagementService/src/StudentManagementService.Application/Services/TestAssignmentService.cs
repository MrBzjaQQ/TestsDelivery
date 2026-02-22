using CommunityToolkit.Diagnostics;
using Microsoft.Extensions.Logging;
using StudentManagementService.Application.Contracts;
using StudentManagementService.Application.DTOs.Requests;
using StudentManagementService.Application.DTOs.Responses;
using StudentManagementService.Application.Infrastructure.Database.Contract;
using StudentManagementService.Domain.Entities;
using StudentManagementService.Domain.Exceptions;

namespace StudentManagementService.Application.Services;

public class TestAssignmentService : ITestAssignmentService
{
    private readonly ITestAssignmentRepository _assignmentRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly ITestProgressRepository _progressRepository;
    private readonly ILogger<TestAssignmentService> _logger;

    public TestAssignmentService(
        ITestAssignmentRepository assignmentRepository,
        IStudentRepository studentRepository,
        ITestProgressRepository progressRepository,
        ILogger<TestAssignmentService> logger)
    {
        _assignmentRepository = assignmentRepository;
        _studentRepository = studentRepository;
        _progressRepository = progressRepository;
        _logger = logger;
    }

    public async Task<TestAssignmentDto> AssignTestAsync(Guid studentId, Guid testId, AssignTestRequest request, CancellationToken ct)
    {
        Guard.IsNotNull(request);

        var student = await _studentRepository.GetByIdAsync(studentId, ct);
        if (student == null)
        {
            throw new StudentNotFoundException(studentId);
        }

        var existingAssignment = await _assignmentRepository.GetByStudentAndTestAsync(studentId, testId, ct);
        if (existingAssignment != null)
        {
            throw new StudentAlreadyEnrolledException(studentId, testId);
        }

        var assignment = new TestAssignment
        {
            Id = Guid.NewGuid(),
            StudentId = studentId,
            TestId = testId,
            AssignedAt = DateTime.UtcNow,
            Deadline = request.Deadline,
            AttemptsAllowed = request.AttemptsAllowed,
            Status = TestAssignmentStatus.Assigned,
            CreatedAt = DateTime.UtcNow
        };

        await _assignmentRepository.AddAsync(assignment, ct);

        _logger.LogInformation("Test {TestId} assigned to student {StudentId}", testId, studentId);

        return MapToDto(assignment);
    }

    public async Task<TestAssignmentListDto> GetStudentTestsAsync(Guid studentId, CancellationToken ct)
    {
        var student = await _studentRepository.GetByIdAsync(studentId, ct);
        if (student == null)
        {
            throw new StudentNotFoundException(studentId);
        }

        var assignments = await _assignmentRepository.GetByStudentIdAsync(studentId, ct);

        return new TestAssignmentListDto
        {
            Assignments = assignments.Select(MapToDto).ToList(),
            TotalCount = assignments.Count
        };
    }

    public async Task<TestAssignmentListDto> GetActiveTestsAsync(Guid studentId, CancellationToken ct)
    {
        var student = await _studentRepository.GetByIdAsync(studentId, ct);
        if (student == null)
        {
            throw new StudentNotFoundException(studentId);
        }

        var assignments = await _assignmentRepository.GetActiveByStudentIdAsync(studentId, ct);

        return new TestAssignmentListDto
        {
            Assignments = assignments.Select(MapToDto).ToList(),
            TotalCount = assignments.Count
        };
    }

    public async Task<SubmitTestResponseDto> SubmitTestAsync(Guid studentId, Guid testId, SubmitTestRequest request, CancellationToken ct)
    {
        Guard.IsNotNull(request);

        var assignment = await _assignmentRepository.GetByStudentAndTestAsync(studentId, testId, ct);
        if (assignment == null)
        {
            throw new TestAssignmentNotFoundException(studentId, testId);
        }

        var violations = new List<string>();

        if (assignment.Deadline.HasValue && assignment.Deadline.Value < DateTime.UtcNow)
        {
            violations.Add($"Test deadline: {assignment.Deadline.Value:yyyy-MM-dd HH:mm:ss}");
        }

        if (assignment.AttemptsUsed >= assignment.AttemptsAllowed)
        {
            violations.Add($"Maximum attempts reached: {assignment.AttemptsUsed}/{assignment.AttemptsAllowed}");
        }

        if (violations.Count > 0)
        {
            throw new TestNotEligibleException("Test deadline has passed or maximum attempts reached", violations);
        }

        var latestProgress = await _progressRepository.GetLatestByStudentAndTestAsync(studentId, testId, ct);
        var attemptNumber = (short)((latestProgress?.AttemptNumber ?? 0) + 1);

        var progress = new TestProgress
        {
            Id = Guid.NewGuid(),
            StudentId = studentId,
            TestId = testId,
            AttemptNumber = attemptNumber,
            MaxScore = 100,
            SubmittedAt = DateTime.UtcNow,
            Answers = request.Answers.Select(a => new Answer
            {
                QuestionId = a.QuestionId,
                SelectedOptionId = a.SelectedOptionId,
                TextAnswer = a.TextAnswer
            }).ToList(),
            CreatedAt = DateTime.UtcNow
        };

        await _progressRepository.AddAsync(progress, ct);

        assignment.AttemptsUsed++;
        assignment.Status = TestAssignmentStatus.InProgress;
        await _assignmentRepository.UpdateAsync(assignment, ct);

        _logger.LogInformation("Test {TestId} submitted by student {StudentId}, attempt {AttemptNumber}", testId, studentId, attemptNumber);

        return new SubmitTestResponseDto
        {
            AttemptNumber = attemptNumber,
            Status = "Submitted",
            SubmittedAt = progress.SubmittedAt!.Value
        };
    }

    private static TestAssignmentDto MapToDto(TestAssignment assignment)
    {
        return new TestAssignmentDto
        {
            Id = assignment.Id,
            StudentId = assignment.StudentId,
            TestId = assignment.TestId,
            AssignedAt = assignment.AssignedAt,
            Deadline = assignment.Deadline,
            Status = assignment.Status.ToString(),
            AttemptsAllowed = assignment.AttemptsAllowed,
            AttemptsUsed = assignment.AttemptsUsed
        };
    }
}
