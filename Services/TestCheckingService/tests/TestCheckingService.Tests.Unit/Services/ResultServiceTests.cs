using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using TestCheckingService.Application.Infrastructure.Database.Contract;
using TestCheckingService.Application.Contracts;
using TestCheckingService.Application.DTOs.Responses;
using TestCheckingService.Domain.Entities;
using TestCheckingService.Domain.Exceptions;
using Xunit;

namespace TestCheckingService.Tests.Unit.Services;

public class ResultServiceTests
{
    private readonly Mock<ITestResultRepository> _mockTestResultRepository;
    private readonly Mock<ITestRepository> _mockTestRepository;
    private readonly Mock<ILogger<ResultService>> _mockLogger;
    private readonly ResultService _service;

    public ResultServiceTests()
    {
        _mockTestResultRepository = new Mock<ITestResultRepository>();
        _mockTestRepository = new Mock<ITestRepository>();
        _mockLogger = new Mock<ILogger<ResultService>>();
        _service = new ResultService(
            _mockTestResultRepository.Object,
            _mockTestRepository.Object,
            _mockLogger.Object);
    }

    [Fact]
    public async Task GetResultAsync_Should_ReturnResult_WhenFound()
    {
        var testId = Guid.NewGuid();
        var studentId = Guid.NewGuid();

        _mockTestResultRepository.Setup(r => r.GetByIdAsync(testId, studentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new TestResult
            {
                Id = Guid.NewGuid(),
                TestId = testId,
                StudentId = studentId,
                Score = 85,
                MaxScore = 100,
                Percentage = 85,
                IsPassed = true,
                AttemptNumber = 1,
                Answers = "[]",
                CreatedAt = DateTime.UtcNow
            });

        var result = await _service.GetResultAsync(testId, studentId, CancellationToken.None);

        result.Score.Should().Be(85);
        result.IsPassed.Should().BeTrue();
    }

    [Fact]
    public async Task GetResultAsync_Should_ThrowException_WhenNotFound()
    {
        var testId = Guid.NewGuid();
        var studentId = Guid.NewGuid();

        _mockTestResultRepository.Setup(r => r.GetByIdAsync(testId, studentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((TestResult?)null);

        var act = async () => await _service.GetResultAsync(testId, studentId, CancellationToken.None);

        await act.Should().ThrowAsync<TestResultNotFoundException>();
    }

    [Fact]
    public async Task GetTestResultsAsync_Should_ReturnAllResultsForTest()
    {
        var testId = Guid.NewGuid();

        _mockTestRepository.Setup(r => r.GetByIdAsync(testId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Test
            {
                Id = testId,
                Title = "Test Title",
                PassPercentage = 70,
                MaxScore = 100,
                CreatedAt = DateTime.UtcNow
            });

        _mockTestResultRepository.Setup(r => r.GetByTestIdAsync(testId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(
            [
                new TestResult
                {
                    Id = Guid.NewGuid(),
                    TestId = testId,
                    StudentId = Guid.NewGuid(),
                    Score = 85,
                    MaxScore = 100,
                    Percentage = 85,
                    IsPassed = true,
                    AttemptNumber = 1,
                    Answers = "[]",
                    CreatedAt = DateTime.UtcNow
                },
                new TestResult
                {
                    Id = Guid.NewGuid(),
                    TestId = testId,
                    StudentId = Guid.NewGuid(),
                    Score = 50,
                    MaxScore = 100,
                    Percentage = 50,
                    IsPassed = false,
                    AttemptNumber = 1,
                    Answers = "[]",
                    CreatedAt = DateTime.UtcNow
                }
            ]);

        var result = await _service.GetTestResultsAsync(testId, CancellationToken.None);

        result.TestId.Should().Be(testId);
        result.TestTitle.Should().Be("Test Title");
        result.TotalResults.Should().Be(2);
        result.PassedResults.Should().Be(1);
        result.FailedResults.Should().Be(1);
    }

    [Fact]
    public async Task GetStudentResultsAsync_Should_ReturnAllResultsForStudent()
    {
        var studentId = Guid.NewGuid();

        _mockTestResultRepository.Setup(r => r.GetByStudentIdAsync(studentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(
            [
                new TestResult
                {
                    Id = Guid.NewGuid(),
                    TestId = Guid.NewGuid(),
                    StudentId = studentId,
                    Score = 85,
                    MaxScore = 100,
                    Percentage = 85,
                    IsPassed = true,
                    AttemptNumber = 1,
                    Answers = "[]",
                    CreatedAt = DateTime.UtcNow
                }
            ]);

        var result = await _service.GetStudentResultsAsync(studentId, CancellationToken.None);

        result.StudentId.Should().Be(studentId);
        result.TotalTests.Should().Be(1);
        result.PassedTests.Should().Be(1);
        result.FailedTests.Should().Be(0);
    }
}
