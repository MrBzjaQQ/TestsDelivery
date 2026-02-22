using BffPortalService.Application.DTOs.Requests;
using BffPortalService.Application.DTOs.Responses;
using Refit;

namespace BffPortalService.Infrastructure.HttpClients.External.Clients;

public interface ITestCheckingServiceClient
{
    [Get("/api/v1/tests/available")]
    Task<IApiResponse<AvailableTestsDto>> GetAvailableTestsAsync(
        [Query] Guid userId,
        [Query] Guid? groupId,
        [Query] string? status,
        [Header("Authorization")] string authorization);

    [Post("/api/v1/tests/{testId}/start")]
    Task<IApiResponse<StartTestResponseDto>> StartTestAsync(
        Guid testId,
        [Body] StartTestRequestDto request,
        [Header("Authorization")] string authorization);

    [Post("/api/v1/tests/{testId}/submit")]
    Task<IApiResponse<SubmitTestResponseDto>> SubmitTestAsync(
        Guid testId,
        [Body] SubmitTestRequestDto request,
        [Header("Authorization")] string authorization);

    [Get("/api/v1/tests/{testId}/results")]
    Task<IApiResponse<TestResultsDto>> GetTestResultsAsync(
        Guid testId,
        [Query] Guid studentId,
        [Header("Authorization")] string authorization);

    [Get("/api/v1/students/{studentId}/results")]
    Task<IApiResponse<List<RecentTestDto>>> GetStudentResultsAsync(
        Guid studentId,
        [Header("Authorization")] string authorization);

    [Get("/api/v1/groups/{groupId}/analytics")]
    Task<IApiResponse<GroupAnalyticsData>> GetGroupAnalyticsAsync(
        Guid groupId,
        [Header("Authorization")] string authorization);
}

public record StartTestRequestDto
{
    public Guid StudentId { get; init; }
}

public record SubmitTestRequestDto
{
    public Guid StudentId { get; init; }
    public List<AnswerItemDto> Answers { get; init; } = [];
}

public record AnswerItemDto
{
    public Guid QuestionId { get; init; }
    public List<Guid> SelectedOptionIds { get; init; } = [];
    public string? TextAnswer { get; init; }
}

public record GroupAnalyticsData
{
    public int CompletedTests { get; init; }
    public double AverageScore { get; init; }
    public double PassRate { get; init; }
    public List<TopPerformerDto> TopPerformers { get; init; } = [];
    public GroupProgressDto? GroupProgress { get; init; }
}
