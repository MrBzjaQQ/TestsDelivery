using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using QuestionManagementService.Application.DTOs.Requests;
using QuestionManagementService.Application.Infrastructure.Database.Contract;
using QuestionManagementService.Application.Services;
using QuestionManagementService.Domain.Entities;
using QuestionManagementService.Domain.Exceptions;
using Xunit;

namespace QuestionManagementService.Tests.Unit.Services;

public class TestTemplateServiceTests
{
    private readonly Mock<ITestTemplateRepository> _mockTemplateRepository;
    private readonly Mock<ILogger<TestTemplateService>> _mockLogger;
    private readonly TestTemplateService _service;

    public TestTemplateServiceTests()
    {
        _mockTemplateRepository = new Mock<ITestTemplateRepository>();
        _mockLogger = new Mock<ILogger<TestTemplateService>>();
        _service = new TestTemplateService(_mockTemplateRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task CreateTemplateAsync_Should_ReturnSuccess_When_Valid()
    {
        var request = new CreateTestTemplateRequest
        {
            Name = "Standard Math Exam",
            Description = "Standard template",
            DefaultDuration = 60,
            DefaultPassingScore = 70
        };

        _mockTemplateRepository.Setup(r => r.AddAsync(It.IsAny<TestTemplate>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await _service.CreateTemplateAsync(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Name.Should().Be(request.Name);
        result.DefaultDuration.Should().Be(request.DefaultDuration);
        _mockTemplateRepository.Verify(r => r.AddAsync(It.IsAny<TestTemplate>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetTemplateByIdAsync_Should_ReturnTemplate_When_Exists()
    {
        var templateId = Guid.NewGuid();
        var template = new TestTemplate
        {
            Id = templateId,
            Name = "Standard Math Exam",
            Description = "Standard template",
            DefaultDuration = 60,
            DefaultPassingScore = 70,
            CreatedAt = DateTime.UtcNow
        };

        _mockTemplateRepository.Setup(r => r.GetByIdAsync(templateId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(template);

        var result = await _service.GetTemplateByIdAsync(templateId, CancellationToken.None);

        result.Should().NotBeNull();
        result.Id.Should().Be(templateId);
        result.Name.Should().Be("Standard Math Exam");
    }

    [Fact]
    public async Task GetTemplateByIdAsync_Should_ThrowNotFoundException_When_NotExists()
    {
        var templateId = Guid.NewGuid();
        _mockTemplateRepository.Setup(r => r.GetByIdAsync(templateId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((TestTemplate?)null);

        await Assert.ThrowsAsync<TestTemplateNotFoundException>(() =>
            _service.GetTemplateByIdAsync(templateId, CancellationToken.None));
    }

    [Fact]
    public async Task UpdateTemplateAsync_Should_UpdateTemplate_When_Exists()
    {
        var templateId = Guid.NewGuid();
        var existingTemplate = new TestTemplate
        {
            Id = templateId,
            Name = "Old Name",
            Description = "Old Description",
            DefaultDuration = 30,
            DefaultPassingScore = 60,
            CreatedAt = DateTime.UtcNow.AddDays(-1)
        };

        var request = new CreateTestTemplateRequest
        {
            Name = "New Name",
            Description = "New Description",
            DefaultDuration = 90,
            DefaultPassingScore = 80
        };

        _mockTemplateRepository.Setup(r => r.GetByIdAsync(templateId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingTemplate);

        _mockTemplateRepository.Setup(r => r.UpdateAsync(It.IsAny<TestTemplate>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await _service.UpdateTemplateAsync(templateId, request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Name.Should().Be("New Name");
        _mockTemplateRepository.Verify(r => r.UpdateAsync(It.IsAny<TestTemplate>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
