using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NotificationService.Application.Contracts;
using NotificationService.WebApi.Controllers;
using NotificationService.WebApi.Shared;
using Xunit;

namespace NotificationService.Tests.Unit.Controllers;

public class TemplatesControllerTests
{
    private readonly Mock<ITemplateService> _mockTemplateService;
    private readonly TemplatesController _controller;

    public TemplatesControllerTests()
    {
        _mockTemplateService = new Mock<ITemplateService>();
        _controller = new TemplatesController(_mockTemplateService.Object);
    }

    [Fact]
    public async Task TemplateExists_Should_ReturnTrue_When_TemplateExists()
    {
        _mockTemplateService.Setup(s => s.TemplateExistsAsync("test-template", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var result = await _controller.TemplateExists("test-template", CancellationToken.None);

        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeOfType<ResponseResultModel<bool>>().Subject;
        response.IsError.Should().BeFalse();
        response.Data.Should().BeTrue();
    }

    [Fact]
    public async Task TemplateExists_Should_ReturnFalse_When_TemplateDoesNotExist()
    {
        _mockTemplateService.Setup(s => s.TemplateExistsAsync("non-existent", It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var result = await _controller.TemplateExists("non-existent", CancellationToken.None);

        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeOfType<ResponseResultModel<bool>>().Subject;
        response.IsError.Should().BeFalse();
        response.Data.Should().BeFalse();
    }
}
