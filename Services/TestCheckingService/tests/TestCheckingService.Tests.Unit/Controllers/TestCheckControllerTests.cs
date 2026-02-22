using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TestCheckingService.Application.Contracts;
using TestCheckingService.Application.DTOs.Requests;
using TestCheckingService.Application.DTOs.Responses;
using TestCheckingService.WebApi.Controllers;
using TestCheckingService.WebApi.Shared;
using Xunit;

namespace TestCheckingService.Tests.Unit.Controllers;

public class TestCheckControllerTests
{
    private readonly Mock<ITestCheckService> _mockTestCheckService;
    private readonly Mock<ILogger<TestCheckController>> _mockLogger;
    private readonly TestCheckController _controller;

    public TestCheckControllerTests()
    {
        _mockTestCheckService = new Mock<ITestCheckService>();
        _mockLogger = new Mock<ILogger<TestCheckController>>();
        _controller = new TestCheckController(_mockTestCheckService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task CheckTest_Should_ReturnOk_WithResult()
    {
        var testId = Guid.NewGuid();
        var request = new CheckTestRequest
        {
            StudentId = Guid.NewGuid(),
            Answers = [new AnswerDto { QuestionId = Guid.NewGuid(), SelectedOptionId = Guid.NewGuid() }]
        };

        var expectedResult = new TestResultDto
        {
            TestId = testId,
            StudentId = request.StudentId,
            Score = 85,
            MaxScore = 100,
            Percentage = 85,
            IsPassed = true
        };

        _mockTestCheckService.Setup(s => s.CheckTestAsync(testId, request.StudentId, request.Answers, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var result = await _controller.CheckTest(testId, request, CancellationToken.None);

        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        var response = okResult!.Value as ResponseResultModel<TestResultDto>;
        response.Should().NotBeNull();
        response!.IsError.Should().BeFalse();
        response.Data.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task BatchCheck_Should_ReturnOk_WithResults()
    {
        var request = new BatchCheckRequest
        {
            Checks =
            [
                new BatchCheckItem
                {
                    TestId = Guid.NewGuid(),
                    StudentId = Guid.NewGuid(),
                    Answers = []
                }
            ]
        };

        var expectedResult = new BatchCheckResultDto
        {
            TotalChecks = 1,
            SuccessfulChecks = 1,
            FailedChecks = 0,
            Results =
            [
                new TestResultDto
                {
                    TestId = request.Checks[0].TestId,
                    StudentId = request.Checks[0].StudentId,
                    Score = 85,
                    MaxScore = 100,
                    Percentage = 85,
                    IsPassed = true
                }
            ]
        };

        _mockTestCheckService.Setup(s => s.BatchCheckAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var result = await _controller.BatchCheck(request, CancellationToken.None);

        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        var response = okResult!.Value as ResponseResultModel<BatchCheckResultDto>;
        response.Should().NotBeNull();
        response!.IsError.Should().BeFalse();
        response.Data.Should().BeEquivalentTo(expectedResult);
    }
}
