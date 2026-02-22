using Microsoft.Extensions.Logging;
using StudentManagementService.Application.Contracts;
using StudentManagementService.Application.DTOs.Responses;
using StudentManagementService.Application.Infrastructure.Database.Contract;
using StudentManagementService.Domain.Entities;
using StudentManagementService.Domain.Exceptions;

namespace StudentManagementService.Application.Services;

public class ProgressTrackingService : IProgressTrackingService
{
    private readonly IStudentRepository _studentRepository;
    private readonly ITestAssignmentRepository _assignmentRepository;
    private readonly ITestProgressRepository _progressRepository;
    private readonly ILogger<ProgressTrackingService> _logger;

    public ProgressTrackingService(
        IStudentRepository studentRepository,
        ITestAssignmentRepository assignmentRepository,
        ITestProgressRepository progressRepository,
        ILogger<ProgressTrackingService> logger)
    {
        _studentRepository = studentRepository;
        _assignmentRepository = assignmentRepository;
        _progressRepository = progressRepository;
        _logger = logger;
    }

    public async Task<ProgressReportDto> GetStudentProgressAsync(Guid studentId, CancellationToken ct)
    {
        var student = await _studentRepository.GetByIdAsync(studentId, ct);
        if (student == null)
        {
            throw new StudentNotFoundException(studentId);
        }

        var assignments = await _assignmentRepository.GetByStudentIdAsync(studentId, ct);
        var allProgress = await _progressRepository.GetByStudentIdAsync(studentId, ct);

        var completedProgress = allProgress.Where(p => p.Score.HasValue).ToList();

        var report = new ProgressReportDto
        {
            StudentId = studentId,
            TotalTests = assignments.Count,
            CompletedTests = completedProgress.Count,
            InProgressTests = assignments.Count(a => a.Status == TestAssignmentStatus.InProgress),
            AssignedTests = assignments.Count(a => a.Status == TestAssignmentStatus.Assigned),
            AverageScore = completedProgress.Count != 0
                ? completedProgress.Average(p => p.Score!.Value)
                : 0,
            CompletedTestsList = completedProgress.Select(p => new CompletedTestDto
            {
                TestId = p.TestId,
                Score = p.Score!.Value,
                MaxScore = p.MaxScore,
                IsPassed = p.IsPassed,
                CompletedAt = p.SubmittedAt
            }).ToList()
        };

        return report;
    }

    public async Task<TestResultDto> GetTestResultsAsync(Guid studentId, Guid testId, CancellationToken ct)
    {
        var student = await _studentRepository.GetByIdAsync(studentId, ct);
        if (student == null)
        {
            throw new StudentNotFoundException(studentId);
        }

        var progressList = await _progressRepository.GetByStudentAndTestAsync(studentId, testId, ct);

        if (progressList.Count == 0)
        {
            throw new TestAssignmentNotFoundException(studentId, testId);
        }

        return new TestResultDto
        {
            TestId = testId,
            Attempts = progressList.Select(p => new TestAttemptDto
            {
                AttemptNumber = p.AttemptNumber,
                Score = p.Score,
                MaxScore = p.MaxScore,
                IsPassed = p.IsPassed,
                SubmittedAt = p.SubmittedAt,
                Answers = p.Answers?.Select(a => new AnswerResultDto
                {
                    QuestionId = a.QuestionId,
                    SelectedOptionId = a.SelectedOptionId,
                    IsCorrect = a.IsCorrect,
                    PointsEarned = a.PointsEarned
                }).ToList()
            }).ToList()
        };
    }

    public async Task UpdateTestProgressAsync(Guid studentId, Guid testId, short score, bool isPassed, CancellationToken ct)
    {
        var progress = await _progressRepository.GetLatestByStudentAndTestAsync(studentId, testId, ct);
        if (progress == null)
        {
            _logger.LogWarning("No progress found for student {StudentId} and test {TestId}", studentId, testId);
            return;
        }

        progress.Score = score;
        progress.IsPassed = isPassed;

        await _progressRepository.UpdateAsync(progress, ct);

        var assignment = await _assignmentRepository.GetByStudentAndTestAsync(studentId, testId, ct);
        if (assignment != null)
        {
            assignment.Status = TestAssignmentStatus.Completed;
            await _assignmentRepository.UpdateAsync(assignment, ct);
        }

        _logger.LogInformation("Progress updated for student {StudentId} test {TestId}: score={Score}, passed={IsPassed}", studentId, testId, score, isPassed);
    }
}
