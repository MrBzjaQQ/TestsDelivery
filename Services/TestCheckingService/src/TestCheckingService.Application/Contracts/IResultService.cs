using Microsoft.Extensions.Logging;
using TestCheckingService.Application.DTOs.Responses;
using TestCheckingService.Application.Infrastructure.Database.Contract;
using TestCheckingService.Domain.Exceptions;

namespace TestCheckingService.Application.Contracts;

public interface IResultService
{
    Task<TestResultDto> GetResultAsync(Guid testId, Guid studentId, CancellationToken ct);

    Task<TestResultsListDto> GetTestResultsAsync(Guid testId, CancellationToken ct);

    Task<StudentResultsListDto> GetStudentResultsAsync(Guid studentId, CancellationToken ct);

    Task<DetailedTestResultDto> GetDetailedResultAsync(Guid testId, Guid studentId, CancellationToken ct);
}

public class ResultService : IResultService
{
    private readonly ITestResultRepository _testResultRepository;
    private readonly ITestRepository _testRepository;
    private readonly ILogger<ResultService> _logger;

    public ResultService(
        ITestResultRepository testResultRepository,
        ITestRepository testRepository,
        ILogger<ResultService> logger)
    {
        _testResultRepository = testResultRepository;
        _testRepository = testRepository;
        _logger = logger;
    }

    public async Task<TestResultDto> GetResultAsync(Guid testId, Guid studentId, CancellationToken ct)
    {
        var result = await _testResultRepository.GetByIdAsync(testId, studentId, ct);

        if (result == null)
        {
            throw new TestResultNotFoundException(testId, studentId);
        }

        return MapToTestResultDto(result);
    }

    public async Task<TestResultsListDto> GetTestResultsAsync(Guid testId, CancellationToken ct)
    {
        var test = await _testRepository.GetByIdAsync(testId, ct);
        if (test == null)
        {
            throw new TestResultNotFoundException(testId);
        }

        var results = await _testResultRepository.GetByTestIdAsync(testId, ct);

        var passedCount = results.Count(r => r.IsPassed);
        var averageScore = results.Count > 0 ? results.Average(r => r.Score) : 0;

        return new TestResultsListDto
        {
            TestId = testId,
            TestTitle = test.Title,
            TotalResults = results.Count,
            PassedResults = passedCount,
            FailedResults = results.Count - passedCount,
            AverageScore = Math.Round(averageScore, 2),
            Results = results.Select(r => new StudentTestResultDto
            {
                StudentId = r.StudentId,
                StudentName = $"Student {r.StudentId}",
                Score = r.Score,
                MaxScore = r.MaxScore,
                Percentage = (double)r.Percentage,
                IsPassed = r.IsPassed,
                AttemptNumber = r.AttemptNumber,
                CompletedAt = r.CreatedAt
            }).ToList()
        };
    }

    public async Task<StudentResultsListDto> GetStudentResultsAsync(Guid studentId, CancellationToken ct)
    {
        var results = await _testResultRepository.GetByStudentIdAsync(studentId, ct);

        var passedCount = results.Count(r => r.IsPassed);
        var averageScore = results.Count > 0 ? results.Average(r => r.Percentage) : 0;

        return new StudentResultsListDto
        {
            StudentId = studentId,
            StudentName = $"Student {studentId}",
            TotalTests = results.Count,
            PassedTests = passedCount,
            FailedTests = results.Count - passedCount,
            AverageScore = Math.Round((double)averageScore, 2),
            Results = results.Select(r => new StudentResultItemDto
            {
                TestId = r.TestId,
                TestTitle = $"Test {r.TestId}",
                Score = r.Score,
                MaxScore = r.MaxScore,
                Percentage = (double)r.Percentage,
                IsPassed = r.IsPassed,
                AttemptNumber = r.AttemptNumber,
                CompletedAt = r.CreatedAt
            }).ToList()
        };
    }

    public async Task<DetailedTestResultDto> GetDetailedResultAsync(Guid testId, Guid studentId, CancellationToken ct)
    {
        var result = await _testResultRepository.GetByIdWithAnswersAsync(testId, studentId, ct);

        if (result == null)
        {
            throw new TestResultNotFoundException(testId, studentId);
        }

        var test = await _testRepository.GetByIdAsync(testId, ct);

        return new DetailedTestResultDto
        {
            TestId = testId,
            StudentId = studentId,
            TestTitle = test?.Title ?? $"Test {testId}",
            Score = result.Score,
            MaxScore = result.MaxScore,
            Percentage = (double)result.Percentage,
            IsPassed = result.IsPassed,
            AttemptNumber = result.AttemptNumber,
            PassedDate = result.PassedDate,
            Answers = []
        };
    }

    private static TestResultDto MapToTestResultDto(Domain.Entities.TestResult result)
    {
        return new TestResultDto
        {
            TestId = result.TestId,
            StudentId = result.StudentId,
            Score = result.Score,
            MaxScore = result.MaxScore,
            Percentage = (double)result.Percentage,
            IsPassed = result.IsPassed,
            PassedThreshold = 70,
            PassedDate = result.PassedDate,
            AttemptNumber = result.AttemptNumber,
            AnswerResults = []
        };
    }
}
