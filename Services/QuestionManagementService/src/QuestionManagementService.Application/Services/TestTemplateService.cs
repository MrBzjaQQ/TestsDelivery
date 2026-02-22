using CommunityToolkit.Diagnostics;
using Microsoft.Extensions.Logging;
using QuestionManagementService.Application.Contracts;
using QuestionManagementService.Application.DTOs.Requests;
using QuestionManagementService.Application.DTOs.Responses;
using QuestionManagementService.Application.Infrastructure.Database.Contract;
using QuestionManagementService.Domain.Entities;
using QuestionManagementService.Domain.Exceptions;

namespace QuestionManagementService.Application.Services;

public class TestTemplateService : ITestTemplateService
{
    private readonly ITestTemplateRepository _templateRepository;
    private readonly ILogger<TestTemplateService> _logger;

    public TestTemplateService(
        ITestTemplateRepository templateRepository,
        ILogger<TestTemplateService> logger)
    {
        _templateRepository = templateRepository;
        _logger = logger;
    }

    public async Task<TestTemplateDto> CreateTemplateAsync(CreateTestTemplateRequest request, CancellationToken ct)
    {
        Guard.IsNotNull(request);

        var template = new TestTemplate
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            DefaultDuration = request.DefaultDuration,
            DefaultPassingScore = request.DefaultPassingScore,
            Configuration = request.Configuration,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        await _templateRepository.AddAsync(template, ct);

        _logger.LogInformation("Test template created: {TemplateId}", template.Id);

        return MapToDto(template);
    }

    public async Task<TestTemplateDto> GetTemplateByIdAsync(Guid id, CancellationToken ct)
    {
        var template = await _templateRepository.GetByIdAsync(id, ct);
        if (template == null)
        {
            throw new TestTemplateNotFoundException(id);
        }

        return MapToDto(template);
    }

    public async Task<TestTemplateDto> UpdateTemplateAsync(Guid id, CreateTestTemplateRequest request, CancellationToken ct)
    {
        Guard.IsNotNull(request);

        var template = await _templateRepository.GetByIdAsync(id, ct);
        if (template == null)
        {
            throw new TestTemplateNotFoundException(id);
        }

        var updatedTemplate = new TestTemplate
        {
            Id = template.Id,
            Name = request.Name,
            Description = request.Description,
            DefaultDuration = request.DefaultDuration,
            DefaultPassingScore = request.DefaultPassingScore,
            Configuration = request.Configuration,
            CreatedAt = template.CreatedAt,
            IsDeleted = template.IsDeleted
        };

        await _templateRepository.UpdateAsync(updatedTemplate, ct);

        _logger.LogInformation("Test template updated: {TemplateId}", id);

        return MapToDto(updatedTemplate);
    }

    private static TestTemplateDto MapToDto(TestTemplate template)
    {
        return new TestTemplateDto
        {
            Id = template.Id,
            Name = template.Name,
            Description = template.Description,
            DefaultDuration = template.DefaultDuration,
            DefaultPassingScore = template.DefaultPassingScore,
            Configuration = template.Configuration,
            CreatedAt = template.CreatedAt
        };
    }
}
