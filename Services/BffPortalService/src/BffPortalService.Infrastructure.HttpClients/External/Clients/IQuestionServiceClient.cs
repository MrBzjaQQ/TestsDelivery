using BffPortalService.Application.DTOs.Responses;
using Refit;

namespace BffPortalService.Infrastructure.HttpClients.External.Clients;

public interface IQuestionServiceClient
{
    [Get("/api/v1/tests/{testId}/questions")]
    Task<IApiResponse<TestQuestionsDto>> GetTestQuestionsAsync(Guid testId, [Header("Authorization")] string authorization);

    [Get("/api/v1/tests/{testId}")]
    Task<IApiResponse<TestDetailsDto>> GetTestAsync(Guid testId, [Header("Authorization")] string authorization);
}

public record TestDetailsDto
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public int DurationMinutes { get; init; }
    public int PassingScore { get; init; }
    public int MaxAttempts { get; init; }
    public int QuestionsCount { get; init; }
    public int TotalPoints { get; init; }
}
