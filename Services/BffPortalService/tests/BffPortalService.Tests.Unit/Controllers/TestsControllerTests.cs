using BffPortalService.Application.Contracts;
using BffPortalService.Application.DTOs.Requests;
using BffPortalService.Application.DTOs.Responses;
using BffPortalService.WebApi.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;
using Xunit;

namespace BffPortalService.Tests.Unit.Controllers;

public class TestsControllerTests
{
    private readonly Mock<ITestsPortalService> _testsPortalServiceMock;
    private readonly Mock<ILogger<TestsController>> _loggerMock;
    private readonly TestsController _controller;

    public TestsControllerTests()
    {
        _testsPortalServiceMock = new Mock<ITestsPortalService>();
        _loggerMock = new Mock<ILogger<TestsController>>();
        _controller = new TestsController(_testsPortalServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetAvailableTests_WhenSuccessful_ShouldReturnOkResult()
    {
        var userId = Guid.NewGuid();
        var expectedTests = new AvailableTestsDto
        {
            Tests =
            [
                new AvailableTestDto
                {
                    TestId = Guid.NewGuid(),
                    TestTitle = "Test 1",
                    Status = "Available"
                }
            ],
            TotalCount = 1
        };

        _testsPortalServiceMock
            .Setup(x => x.GetAvailableTestsAsync(
                It.IsAny<Guid>(),
                It.IsAny<string>(),
                It.IsAny<GetTestsRequest>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedTests);

        SetupControllerContext(_controller, userId);

        var result = await _controller.GetAvailableTests(null, null, CancellationToken.None);

        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task StartTest_WhenSuccessful_ShouldReturnOkResult()
    {
        var testId = Guid.NewGuid();
        var studentId = Guid.NewGuid();
        var expectedResponse = new StartTestResponseDto
        {
            TestId = testId,
            StudentId = studentId,
            Status = "InProgress",
            StartedAt = DateTime.UtcNow,
            Deadline = DateTime.UtcNow.AddHours(1)
        };

        _testsPortalServiceMock
            .Setup(x => x.StartTestAsync(testId, studentId, It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        SetupControllerContext(_controller, studentId);

        var result = await _controller.StartTest(testId, CancellationToken.None);

        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task GetTestQuestions_WhenSuccessful_ShouldReturnOkResult()
    {
        var testId = Guid.NewGuid();
        var expectedQuestions = new TestQuestionsDto
        {
            TestId = testId,
            TestTitle = "Test 1",
            TotalQuestions = 10
        };

        _testsPortalServiceMock
            .Setup(x => x.GetTestQuestionsAsync(testId, It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedQuestions);

        SetupControllerContext(_controller, Guid.NewGuid());

        var result = await _controller.GetTestQuestions(testId, CancellationToken.None);

        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task SubmitTest_WhenSuccessful_ShouldReturnOkResult()
    {
        var testId = Guid.NewGuid();
        var studentId = Guid.NewGuid();
        var request = new SubmitTestRequestDto
        {
            Answers =
            [
                new AnswerItemDto
                {
                    QuestionId = Guid.NewGuid(),
                    SelectedOptionIds = [Guid.NewGuid()]
                }
            ]
        };

        var expectedResponse = new SubmitTestResponseDto
        {
            TestId = testId,
            StudentId = studentId,
            Status = "Submitted"
        };

        _testsPortalServiceMock
            .Setup(x => x.SubmitTestAnswersAsync(
                testId,
                studentId,
                It.IsAny<string>(),
                It.IsAny<SubmitTestRequest>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        SetupControllerContext(_controller, studentId);

        var result = await _controller.SubmitTest(testId, request, CancellationToken.None);

        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task GetTestResults_WhenSuccessful_ShouldReturnOkResult()
    {
        var testId = Guid.NewGuid();
        var studentId = Guid.NewGuid();
        var expectedResults = new TestResultsDto
        {
            TestId = testId,
            TestTitle = "Test 1",
            Score = 85,
            MaxScore = 100,
            IsPassed = true
        };

        _testsPortalServiceMock
            .Setup(x => x.GetTestResultsAsync(testId, studentId, It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResults);

        SetupControllerContext(_controller, studentId);

        var result = await _controller.GetTestResults(testId, CancellationToken.None);

        result.Should().BeOfType<OkObjectResult>();
    }

    private static void SetupControllerContext(ControllerBase controller, Guid userId)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId.ToString()),
            new("sub", userId.ToString())
        };

        var identity = new ClaimsIdentity(claims, "TestAuth");
        var principal = new ClaimsPrincipal(identity);

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = principal
            }
        };

        controller.HttpContext.Request.Headers.Authorization = "Bearer test-token";
    }
}
