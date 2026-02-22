using QuestionManagementService.Application.DTOs.Requests;
using QuestionManagementService.Application.DTOs.Responses;

namespace QuestionManagementService.Application.Contracts;

public interface ITestTemplateService
{
    Task<TestTemplateDto> CreateTemplateAsync(CreateTestTemplateRequest request, CancellationToken ct);

    Task<TestTemplateDto> GetTemplateByIdAsync(Guid id, CancellationToken ct);

    Task<TestTemplateDto> UpdateTemplateAsync(Guid id, CreateTestTemplateRequest request, CancellationToken ct);
}
