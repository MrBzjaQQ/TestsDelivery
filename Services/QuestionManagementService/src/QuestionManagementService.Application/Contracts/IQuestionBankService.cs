using QuestionManagementService.Application.DTOs.Requests;
using QuestionManagementService.Application.DTOs.Responses;

namespace QuestionManagementService.Application.Contracts;

public interface IQuestionBankService
{
    Task<QuestionBankDto> CreateQuestionBankAsync(CreateQuestionBankRequest request, CancellationToken ct);

    Task<QuestionBankDto> GetQuestionBankByIdAsync(Guid id, CancellationToken ct);

    Task<QuestionListDto> GetQuestionsByBankIdAsync(Guid bankId, CancellationToken ct);
}
