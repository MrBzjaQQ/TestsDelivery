using BffPortalService.Application.Contracts;
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

public class GroupsControllerTests
{
    private readonly Mock<IGroupAnalyticsService> _groupAnalyticsServiceMock;
    private readonly Mock<ILogger<GroupsController>> _loggerMock;
    private readonly GroupsController _controller;

    public GroupsControllerTests()
    {
        _groupAnalyticsServiceMock = new Mock<IGroupAnalyticsService>();
        _loggerMock = new Mock<ILogger<GroupsController>>();
        _controller = new GroupsController(_groupAnalyticsServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetGroupAnalytics_WhenSuccessful_ShouldReturnOkResult()
    {
        var groupId = Guid.NewGuid();
        var expectedAnalytics = new GroupAnalyticsDto
        {
            GroupId = groupId,
            GroupName = "Test Group",
            TotalStudents = 30,
            ActiveStudents = 28,
            CompletedTests = 150,
            AverageScore = 78.5,
            PassRate = 83.3
        };

        _groupAnalyticsServiceMock
            .Setup(x => x.GetGroupAnalyticsAsync(
                It.IsAny<Guid>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedAnalytics);

        SetupControllerContext(_controller);

        var result = await _controller.GetGroupAnalytics(groupId, CancellationToken.None);

        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task GetGroupAnalytics_ShouldCallServiceWithCorrectParameters()
    {
        var groupId = Guid.NewGuid();
        var expectedAnalytics = new GroupAnalyticsDto
        {
            GroupId = groupId,
            GroupName = "Test Group",
            TotalStudents = 30
        };

        _groupAnalyticsServiceMock
            .Setup(x => x.GetGroupAnalyticsAsync(groupId, It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedAnalytics);

        SetupControllerContext(_controller);

        await _controller.GetGroupAnalytics(groupId, CancellationToken.None);

        _groupAnalyticsServiceMock.Verify(
            x => x.GetGroupAnalyticsAsync(groupId, It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetGroupAnalytics_WhenServiceUnavailable_ShouldThrow()
    {
        var groupId = Guid.NewGuid();

        _groupAnalyticsServiceMock
            .Setup(x => x.GetGroupAnalyticsAsync(
                It.IsAny<Guid>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new BffPortalService.Domain.Exceptions.ServiceUnavailableException("StudentService", "Service unavailable"));

        SetupControllerContext(_controller);

        await Assert.ThrowsAsync<BffPortalService.Domain.Exceptions.ServiceUnavailableException>(() =>
            _controller.GetGroupAnalytics(groupId, CancellationToken.None));
    }

    private static void SetupControllerContext(ControllerBase controller)
    {
        var userId = Guid.NewGuid();
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
