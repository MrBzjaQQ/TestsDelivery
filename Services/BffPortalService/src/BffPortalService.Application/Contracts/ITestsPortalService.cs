using BffPortalService.Application.DTOs.Requests;
using BffPortalService.Application.DTOs.Responses;

namespace BffPortalService.Application.Contracts;

public interface ITestsPortalService
{
    Task<AvailableTestsDto> GetAvailableTestsAsync(Guid userId, string accessToken, GetTestsRequest request, CancellationToken ct);

    Task<StartTestResponseDto> StartTestAsync(Guid testId, Guid studentId, string accessToken, CancellationToken ct);

    Task<TestQuestionsDto> GetTestQuestionsAsync(Guid testId, string accessToken, CancellationToken ct);

    Task<SubmitTestResponseDto> SubmitTestAnswersAsync(Guid testId, Guid studentId, string accessToken, SubmitTestRequest request, CancellationToken ct);

    Task<TestResultsDto> GetTestResultsAsync(Guid testId, Guid studentId, string accessToken, CancellationToken ct);

    Task InvalidateTestsCacheAsync(Guid userId, CancellationToken ct);
}
