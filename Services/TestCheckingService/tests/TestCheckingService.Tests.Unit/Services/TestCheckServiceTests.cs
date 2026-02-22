using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using TestCheckingService.Application.Contracts;
using TestCheckingService.Application.DTOs.Requests;
using TestCheckingService.Application.Infrastructure.Database.Contract;
using TestCheckingService.Application.Scoring;
using TestCheckingService.Application.Settings;
using TestCheckingService.Domain.Entities;
using TestCheckingService.Domain.Exceptions;
using Xunit;

namespace TestCheckingService.Tests.Unit.Services;

public class TestCheckServiceTests
{
    private readonly Mock<ITestRepository> _mockTestRepository;
    private readonly Mock<ITestResultRepository> _mockTestResultRepository;
    private readonly Mock<IScoringEngine> _mockScoringEngine;
    private readonly ScoringSettings _scoringSettings;
    private readonly Mock<ILogger<TestCheckService>> _mockLogger;
    private readonly TestCheckService _service;

    public TestCheckServiceTests()
    {
        _mockTestRepository = new Mock<ITestRepository>();
        _mockTestResultRepository = new Mock<ITestResultRepository>();
        _mockScoringEngine = new Mock<IScoringEngine>();
        _scoringSettings = new ScoringSettings
        {
            PassPercentage = 70,
            AllowRetries = true,
            MaxAttempts = 3
        };
        _mockLogger = new Mock<ILogger<TestCheckService>>();
        _service = new TestCheckService(
            _mockTestRepository.Object,
            _mockTestResultRepository.Object,
            _mockScoringEngine.Object,
            _scoringSettings,
            _mockLogger.Object);
    }

    [Fact]
    public async Task CheckTestAsync_Should_CalculateScoreAndPass_WhenAnswersCorrect()
    {
        var testId = Guid.NewGuid();
        var studentId = Guid.NewGuid();
        var answers = new List<AnswerDto>
        {
            new() { QuestionId = Guid.NewGuid(), SelectedOptionId = Guid.NewGuid() },
            new() { QuestionId = Guid.NewGuid(), SelectedOptionId = Guid.NewGuid() }
        };

        _mockTestRepository.Setup(r => r.GetByIdAsync(testId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Test
            {
                Id = testId,
                Title = "Test",
                PassPercentage = 70,
                MaxScore = 100,
                CreatedAt = DateTime.UtcNow
            });

        _mockTestResultRepository.Setup(r => r.GetAttemptCountAsync(testId, studentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);

        _mockScoringEngine.Setup(e => e.ScoreTestAsync(testId, answers, 70, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ScoringResult
            {
                TotalScore = 85,
                MaxScore = 100,
                Percentage = 85,
                IsPassed = true,
                AnswerResults = []
            });

        _mockTestResultRepository.Setup(r => r.AddAsync(It.IsAny<TestResult>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await _service.CheckTestAsync(testId, studentId, answers, CancellationToken.None);

        result.IsPassed.Should().BeTrue();
        result.Percentage.Should().Be(85);
        result.Score.Should().Be(85);
    }

    [Fact]
    public async Task CheckTestAsync_Should_CalculateScoreAndFail_WhenScoreBelowThreshold()
    {
        var testId = Guid.NewGuid();
        var studentId = Guid.NewGuid();
        var answers = new List<AnswerDto>
        {
            new() { QuestionId = Guid.NewGuid(), SelectedOptionId = null }
        };

        _mockTestRepository.Setup(r => r.GetByIdAsync(testId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Test
            {
                Id = testId,
                Title = "Test",
                PassPercentage = 70,
                MaxScore = 100,
                CreatedAt = DateTime.UtcNow
            });

        _mockTestResultRepository.Setup(r => r.GetAttemptCountAsync(testId, studentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);

        _mockScoringEngine.Setup(e => e.ScoreTestAsync(testId, answers, 70, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ScoringResult
            {
                TotalScore = 50,
                MaxScore = 100,
                Percentage = 50,
                IsPassed = false,
                AnswerResults = []
            });

        _mockTestResultRepository.Setup(r => r.AddAsync(It.IsAny<TestResult>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await _service.CheckTestAsync(testId, studentId, answers, CancellationToken.None);

        result.IsPassed.Should().BeFalse();
        result.Percentage.Should().Be(50);
    }

    [Fact]
    public async Task CheckTestAsync_Should_ThrowException_WhenTestNotFound()
    {
        var testId = Guid.NewGuid();
        var studentId = Guid.NewGuid();
        var answers = new List<AnswerDto>();

        _mockTestRepository.Setup(r => r.GetByIdAsync(testId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Test?)null);

        var act = async () => await _service.CheckTestAsync(testId, studentId, answers, CancellationToken.None);

        await act.Should().ThrowAsync<TestResultNotFoundException>();
    }

    [Fact]
    public async Task CheckTestAsync_Should_ThrowException_WhenMaxAttemptsReached()
    {
        var testId = Guid.NewGuid();
        var studentId = Guid.NewGuid();
        var answers = new List<AnswerDto>();

        _mockTestRepository.Setup(r => r.GetByIdAsync(testId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Test
            {
                Id = testId,
                Title = "Test",
                PassPercentage = 70,
                MaxScore = 100,
                CreatedAt = DateTime.UtcNow
            });

        _mockTestResultRepository.Setup(r => r.GetAttemptCountAsync(testId, studentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(3);

        var act = async () => await _service.CheckTestAsync(testId, studentId, answers, CancellationToken.None);

        await act.Should().ThrowAsync<TestNotEligibleException>();
    }

    [Fact]
    public async Task BatchCheckAsync_Should_ProcessAllItems()
    {
        var request = new BatchCheckRequest
        {
            Checks =
            [
                new BatchCheckItem
                {
                    TestId = Guid.NewGuid(),
                    StudentId = Guid.NewGuid(),
                    Answers = [new AnswerDto { QuestionId = Guid.NewGuid(), SelectedOptionId = Guid.NewGuid() }]
                },
                new BatchCheckItem
                {
                    TestId = Guid.NewGuid(),
                    StudentId = Guid.NewGuid(),
                    Answers = [new AnswerDto { QuestionId = Guid.NewGuid(), SelectedOptionId = Guid.NewGuid() }]
                }
            ]
        };

        _mockTestRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Guid id, CancellationToken _) => new Test
            {
                Id = id,
                Title = "Test",
                PassPercentage = 70,
                MaxScore = 100,
                CreatedAt = DateTime.UtcNow
            });

        _mockTestResultRepository.Setup(r => r.GetAttemptCountAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);

        _mockScoringEngine.Setup(e => e.ScoreTestAsync(It.IsAny<Guid>(), It.IsAny<List<AnswerDto>>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ScoringResult
            {
                TotalScore = 85,
                MaxScore = 100,
                Percentage = 85,
                IsPassed = true,
                AnswerResults = []
            });

        _mockTestResultRepository.Setup(r => r.AddAsync(It.IsAny<TestResult>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await _service.BatchCheckAsync(request, CancellationToken.None);

        result.TotalChecks.Should().Be(2);
        result.SuccessfulChecks.Should().Be(2);
        result.FailedChecks.Should().Be(0);
        result.Results.Should().HaveCount(2);
    }
}
