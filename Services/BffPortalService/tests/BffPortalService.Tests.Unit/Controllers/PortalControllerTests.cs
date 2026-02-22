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

public class PortalControllerTests
{
    private readonly Mock<IStudentPortalService> _studentPortalServiceMock;
    private readonly Mock<ILogger<PortalController>> _loggerMock;
    private readonly PortalController _controller;

    public PortalControllerTests()
    {
        _studentPortalServiceMock = new Mock<IStudentPortalService>();
        _loggerMock = new Mock<ILogger<PortalController>>();
        _controller = new PortalController(_studentPortalServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetStudentProfile_WhenSuccessful_ShouldReturnOkResult()
    {
        var studentId = Guid.NewGuid();
        var expectedProfile = new StudentProfileDto
        {
            StudentId = studentId,
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com"
        };

        _studentPortalServiceMock
            .Setup(x => x.GetStudentProfileAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedProfile);

        SetupControllerContext(_controller, studentId);

        var result = await _controller.GetStudentProfile(CancellationToken.None);

        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().NotBeNull();
    }

    [Fact]
    public async Task GetStudentProfile_ShouldCallServiceWithCorrectParameters()
    {
        var studentId = Guid.NewGuid();
        var expectedProfile = new StudentProfileDto
        {
            StudentId = studentId,
            FirstName = "John",
            LastName = "Doe"
        };

        _studentPortalServiceMock
            .Setup(x => x.GetStudentProfileAsync(studentId, It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedProfile);

        SetupControllerContext(_controller, studentId);

        await _controller.GetStudentProfile(CancellationToken.None);

        _studentPortalServiceMock.Verify(
            x => x.GetStudentProfileAsync(studentId, It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once);
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
