using QuestionManagementService.Application.DTOs.Requests;
using QuestionManagementService.Application.DTOs.Responses;

namespace QuestionManagementService.Application.Contracts;

public interface IQuestionService
{
    Task<QuestionDto> CreateQuestionAsync(CreateQuestionRequest request, CancellationToken ct);

    Task<QuestionDto> GetQuestionByIdAsync(Guid id, CancellationToken ct);

    Task<QuestionDto> UpdateQuestionAsync(Guid id, UpdateQuestionRequest request, CancellationToken ct);

    Task DeleteQuestionAsync(Guid id, CancellationToken ct);

    Task<QuestionListDto> GetQuestionsAsync(string? category, string? difficulty, Guid? questionBankId, CancellationToken ct);
}
