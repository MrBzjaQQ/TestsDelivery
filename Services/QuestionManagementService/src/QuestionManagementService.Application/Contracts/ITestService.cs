using QuestionManagementService.Application.DTOs.Requests;
using QuestionManagementService.Application.DTOs.Responses;

namespace QuestionManagementService.Application.Contracts;

public interface ITestService
{
    Task<TestDto> CreateTestAsync(CreateTestRequest request, CancellationToken ct);

    Task<TestDto> GetTestByIdAsync(Guid id, CancellationToken ct);

    Task<TestListDto> GetTestsByTemplateIdAsync(Guid templateId, CancellationToken ct);

    Task<TestDto> CopyTestAsync(Guid id, CancellationToken ct);

    Task<TestDto> GenerateTestFromBankAsync(Guid bankId, GenerateTestRequest request, CancellationToken ct);
}
