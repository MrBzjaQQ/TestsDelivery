using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NotificationService.Application.Exceptions;
using NotificationService.Application.Services;
using Xunit;

namespace NotificationService.Tests.Unit.Services;

public class TemplateServiceTests : IDisposable
{
    private readonly string _testTemplatesPath;
    private readonly TemplateService _service;

    public TemplateServiceTests()
    {
        _testTemplatesPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        var enPath = Path.Combine(_testTemplatesPath, "en");
        Directory.CreateDirectory(enPath);

        File.WriteAllText(Path.Combine(enPath, "test-template.hbs"), "<html>Hello {{Name}}!</html>");

        _service = new TemplateService(_testTemplatesPath, "en", new Mock<ILogger<TemplateService>>().Object);
    }

    public void Dispose()
    {
        if (Directory.Exists(_testTemplatesPath))
        {
            Directory.Delete(_testTemplatesPath, true);
        }
    }

    [Fact]
    public async Task RenderAsync_Should_Render_Template_With_Data()
    {
        var templateData = new { Name = "John" };

        var result = await _service.RenderAsync("test-template", templateData, CancellationToken.None);

        result.Should().Contain("Hello John!");
    }

    [Fact]
    public async Task RenderAsync_Should_ThrowTemplateNotFoundException_When_TemplateDoesNotExist()
    {
        var templateData = new { Name = "John" };

        var act = async () => await _service.RenderAsync("non-existent", templateData, CancellationToken.None);

        await act.Should().ThrowAsync<TemplateNotFoundException>()
            .WithMessage("Template 'non-existent' not found");
    }

    [Fact]
    public async Task TemplateExistsAsync_Should_ReturnTrue_When_TemplateExists()
    {
        var result = await _service.TemplateExistsAsync("test-template", CancellationToken.None);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task TemplateExistsAsync_Should_ReturnFalse_When_TemplateDoesNotExist()
    {
        var result = await _service.TemplateExistsAsync("non-existent", CancellationToken.None);

        result.Should().BeFalse();
    }
}
