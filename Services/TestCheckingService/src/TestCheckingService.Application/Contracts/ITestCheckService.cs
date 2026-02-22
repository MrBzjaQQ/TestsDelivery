using System.Text.Json;
using Microsoft.Extensions.Logging;
using TestCheckingService.Application.DTOs.Requests;
using TestCheckingService.Application.DTOs.Responses;
using TestCheckingService.Application.Infrastructure.Database.Contract;
using TestCheckingService.Application.Scoring;
using TestCheckingService.Application.Settings;
using TestCheckingService.Domain.Entities;
using TestCheckingService.Domain.Exceptions;

namespace TestCheckingService.Application.Contracts;

public interface ITestCheckService
{
    Task<TestResultDto> CheckTestAsync(
        Guid testId,
        Guid studentId,
        List<AnswerDto> answers,
        CancellationToken ct);

    Task<BatchCheckResultDto> BatchCheckAsync(
        BatchCheckRequest request,
        CancellationToken ct);
}

public class TestCheckService : ITestCheckService
{
    private readonly ITestRepository _testRepository;
    private readonly ITestResultRepository _testResultRepository;
    private readonly IScoringEngine _scoringEngine;
    private readonly ScoringSettings _scoringSettings;
    private readonly ILogger<TestCheckService> _logger;

    public TestCheckService(
        ITestRepository testRepository,
        ITestResultRepository testResultRepository,
        IScoringEngine scoringEngine,
        ScoringSettings scoringSettings,
        ILogger<TestCheckService> logger)
    {
        _testRepository = testRepository;
        _testResultRepository = testResultRepository;
        _scoringEngine = scoringEngine;
        _scoringSettings = scoringSettings;
        _logger = logger;
    }

    public async Task<TestResultDto> CheckTestAsync(
        Guid testId,
        Guid studentId,
        List<AnswerDto> answers,
        CancellationToken ct)
    {
        _logger.LogInformation("Checking test {TestId} for student {StudentId}", testId, studentId);

        var test = await _testRepository.GetByIdAsync(testId, ct);
        if (test == null)
        {
            throw new TestResultNotFoundException(testId);
        }

        var attemptCount = await _testResultRepository.GetAttemptCountAsync(testId, studentId, ct);
        if (_scoringSettings.MaxAttempts > 0 && attemptCount >= _scoringSettings.MaxAttempts)
        {
            throw new TestNotEligibleException(
                "Maximum attempts reached",
                [new TestEligibilityViolation("attempts", $"Maximum attempts ({_scoringSettings.MaxAttempts}) reached", "MaxAttemptsExceeded")]);
        }

        var scoringResult = await _scoringEngine.ScoreTestAsync(
            testId,
            answers,
            test.PassPercentage,
            ct);

        var testResult = new TestResult
        {
            Id = Guid.NewGuid(),
            TestId = testId,
            StudentId = studentId,
            Score = scoringResult.TotalScore,
            MaxScore = scoringResult.MaxScore,
            Percentage = (decimal)scoringResult.Percentage,
            IsPassed = scoringResult.IsPassed,
            AttemptNumber = attemptCount + 1,
            PassedDate = scoringResult.IsPassed ? DateTime.UtcNow : null,
            Answers = JsonSerializer.Serialize(answers),
            CreatedAt = DateTime.UtcNow
        };

        await _testResultRepository.AddAsync(testResult, ct);

        _logger.LogInformation(
            "Test {TestId} checked for student {StudentId}. Score: {Score}/{MaxScore}, Passed: {IsPassed}",
            testId, studentId, scoringResult.TotalScore, scoringResult.MaxScore, scoringResult.IsPassed);

        return new TestResultDto
        {
            TestId = testId,
            StudentId = studentId,
            Score = scoringResult.TotalScore,
            MaxScore = scoringResult.MaxScore,
            Percentage = scoringResult.Percentage,
            IsPassed = scoringResult.IsPassed,
            PassedThreshold = test.PassPercentage,
            PassedDate = testResult.PassedDate,
            AttemptNumber = testResult.AttemptNumber,
            AnswerResults = scoringResult.AnswerResults.Select(a => new AnswerResultDto
            {
                QuestionId = a.QuestionId,
                SelectedOptionId = a.SelectedOptionId,
                IsCorrect = a.IsCorrect,
                PointsEarned = a.PointsEarned,
                CorrectOptionId = a.CorrectOptionId
            }).ToList()
        };
    }

    public async Task<BatchCheckResultDto> BatchCheckAsync(
        BatchCheckRequest request,
        CancellationToken ct)
    {
        _logger.LogInformation("Processing batch check with {Count} items", request.Checks.Count);

        var results = new List<TestResultDto>();
        var successCount = 0;
        var failedCount = 0;

        foreach (var checkItem in request.Checks)
        {
            try
            {
                var result = await CheckTestAsync(
                    checkItem.TestId,
                    checkItem.StudentId,
                    checkItem.Answers,
                    ct);

                results.Add(result);
                successCount++;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to check test {TestId} for student {StudentId}",
                    checkItem.TestId, checkItem.StudentId);
                failedCount++;
            }
        }

        return new BatchCheckResultDto
        {
            TotalChecks = request.Checks.Count,
            SuccessfulChecks = successCount,
            FailedChecks = failedCount,
            Results = results
        };
    }
}
