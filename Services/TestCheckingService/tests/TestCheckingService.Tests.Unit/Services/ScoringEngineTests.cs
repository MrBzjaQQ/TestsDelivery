using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using TestCheckingService.Application.DTOs.Requests;
using TestCheckingService.Application.Infrastructure.Database.Contract;
using TestCheckingService.Application.Scoring;
using TestCheckingService.Domain.Entities;
using Xunit;

namespace TestCheckingService.Tests.Unit.Services;

public class ScoringEngineTests
{
    private readonly Mock<ITestRepository> _mockTestRepository;
    private readonly Mock<ILogger<ScoringEngine>> _mockLogger;
    private readonly ScoringEngine _scoringEngine;

    public ScoringEngineTests()
    {
        _mockTestRepository = new Mock<ITestRepository>();
        _mockLogger = new Mock<ILogger<ScoringEngine>>();
        _scoringEngine = new ScoringEngine(_mockTestRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task ScoreTestAsync_Should_ReturnCorrectScore_WhenAllAnswersCorrect()
    {
        var testId = Guid.NewGuid();
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

        var result = await _scoringEngine.ScoreTestAsync(testId, answers, 70, CancellationToken.None);

        result.TotalScore.Should().Be(100);
        result.MaxScore.Should().Be(100);
        result.Percentage.Should().Be(100);
        result.IsPassed.Should().BeTrue();
        result.AnswerResults.Should().HaveCount(2);
    }

    [Fact]
    public async Task ScoreTestAsync_Should_ReturnZeroScore_WhenNoCorrectAnswers()
    {
        var testId = Guid.NewGuid();
        var answers = new List<AnswerDto>
        {
            new() { QuestionId = Guid.NewGuid(), SelectedOptionId = null },
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

        var result = await _scoringEngine.ScoreTestAsync(testId, answers, 70, CancellationToken.None);

        result.TotalScore.Should().Be(0);
        result.Percentage.Should().Be(0);
        result.IsPassed.Should().BeFalse();
    }

    [Fact]
    public async Task ScoreTestAsync_Should_ReturnZero_WhenTestNotFound()
    {
        var testId = Guid.NewGuid();
        var answers = new List<AnswerDto>
        {
            new() { QuestionId = Guid.NewGuid(), SelectedOptionId = Guid.NewGuid() }
        };

        _mockTestRepository.Setup(r => r.GetByIdAsync(testId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Test?)null);

        var result = await _scoringEngine.ScoreTestAsync(testId, answers, 70, CancellationToken.None);

        result.TotalScore.Should().Be(0);
        result.MaxScore.Should().Be(0);
        result.Percentage.Should().Be(0);
        result.IsPassed.Should().BeFalse();
    }

    [Fact]
    public async Task ScoreTestAsync_Should_ReturnZero_WhenNoAnswers()
    {
        var testId = Guid.NewGuid();
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

        var result = await _scoringEngine.ScoreTestAsync(testId, answers, 70, CancellationToken.None);

        result.TotalScore.Should().Be(0);
        result.Percentage.Should().Be(0);
        result.IsPassed.Should().BeFalse();
    }

    [Theory]
    [InlineData(2, 2, 70, true)]
    [InlineData(0, 2, 70, false)]
    [InlineData(2, 2, 50, true)]
    [InlineData(0, 2, 50, false)]
    public async Task ScoreTestAsync_Should_CorrectlyDeterminePassFail(
        int correctCount, int totalQuestions, int passPercentage, bool expectedPassed)
    {
        var testId = Guid.NewGuid();
        var answers = new List<AnswerDto>();

        for (int i = 0; i < totalQuestions; i++)
        {
            answers.Add(new AnswerDto
            {
                QuestionId = Guid.NewGuid(),
                SelectedOptionId = i < correctCount ? Guid.NewGuid() : null
            });
        }

        _mockTestRepository.Setup(r => r.GetByIdAsync(testId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Test
            {
                Id = testId,
                Title = "Test",
                PassPercentage = passPercentage,
                MaxScore = 100,
                CreatedAt = DateTime.UtcNow
            });

        var result = await _scoringEngine.ScoreTestAsync(testId, answers, passPercentage, CancellationToken.None);

        result.IsPassed.Should().Be(expectedPassed);
    }
}
