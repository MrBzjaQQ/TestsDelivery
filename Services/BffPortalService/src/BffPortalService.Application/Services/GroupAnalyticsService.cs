using System.Net.Http.Headers;
using System.Net.Http.Json;
using BffPortalService.Application.Contracts;
using BffPortalService.Application.DTOs.Responses;
using BffPortalService.Domain.Exceptions;
using BffPortalService.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace BffPortalService.Application.Services;

public class GroupAnalyticsService : IGroupAnalyticsService
{
    private const string StudentServiceClientName = "StudentService";
    private const string TestCheckingServiceClientName = "TestCheckingService";

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ICacheService _cacheService;
    private readonly ILogger<GroupAnalyticsService> _logger;

    public GroupAnalyticsService(
        IHttpClientFactory httpClientFactory,
        ICacheService cacheService,
        ILogger<GroupAnalyticsService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task<GroupAnalyticsDto> GetGroupAnalyticsAsync(Guid groupId, string accessToken, CancellationToken ct)
    {
        var cacheKey = PortalDataKey.GroupAnalytics(groupId).ToString();

        return await _cacheService.GetOrAddAsync(
            cacheKey,
            async _ => await FetchGroupAnalyticsAsync(groupId, accessToken, ct),
            CacheDuration.GroupAnalytics.Value,
            ct);
    }

    private async Task<GroupAnalyticsDto> FetchGroupAnalyticsAsync(Guid groupId, string accessToken, CancellationToken ct)
    {
        try
        {
            var studentClient = CreateClient(StudentServiceClientName, accessToken);
            var testCheckingClient = CreateClient(TestCheckingServiceClientName, accessToken);

            var groupInfoTask = studentClient.GetAsync($"api/v1/groups/{groupId}", ct);
            var studentsTask = studentClient.GetAsync($"api/v1/groups/{groupId}/students", ct);
            var analyticsTask = testCheckingClient.GetAsync($"api/v1/groups/{groupId}/analytics", ct);

            await Task.WhenAll(groupInfoTask, studentsTask, analyticsTask);

            var groupResponse = groupInfoTask.Result;
            var studentsResponse = studentsTask.Result;
            var analyticsResponse = analyticsTask.Result;

            if (!groupResponse.IsSuccessStatusCode)
            {
                if (groupResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new PortalDataNotFoundException("Group", groupId.ToString());
                }

                _logger.LogError("Failed to fetch group info. Status: {StatusCode}", groupResponse.StatusCode);
                throw new ServiceUnavailableException("StudentService", "Failed to retrieve group information");
            }

            var groupInfo = await groupResponse.Content.ReadFromJsonAsync<GroupDto>(ct);
            var students = studentsResponse.IsSuccessStatusCode
                ? await studentsResponse.Content.ReadFromJsonAsync<List<GroupStudentDto>>(ct) ?? []
                : [];

            var analytics = analyticsResponse.IsSuccessStatusCode
                ? await analyticsResponse.Content.ReadFromJsonAsync<GroupAnalyticsDataDto>(ct)
                : null;

            return new GroupAnalyticsDto
            {
                GroupId = groupId,
                GroupName = groupInfo?.Name ?? string.Empty,
                TotalStudents = students.Count,
                ActiveStudents = students.Count(s => s.IsActive),
                CompletedTests = analytics?.CompletedTests ?? 0,
                AverageScore = analytics?.AverageScore ?? 0,
                PassRate = analytics?.PassRate ?? 0,
                TopPerformers = analytics?.TopPerformers ?? [],
                GroupProgress = analytics?.GroupProgress
            };
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error while fetching group analytics for {GroupId}", groupId);
            throw new ServiceUnavailableException("StudentService", "Group analytics service is temporarily unavailable", ex);
        }
    }

    public async Task InvalidateGroupCacheAsync(Guid groupId, CancellationToken ct)
    {
        var cacheKey = PortalDataKey.GroupAnalytics(groupId).ToString();
        await _cacheService.RemoveAsync(cacheKey, ct);
        _logger.LogInformation("Cache invalidated for group {GroupId}", groupId);
    }

    private HttpClient CreateClient(string clientName, string accessToken)
    {
        var client = _httpClientFactory.CreateClient(clientName);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        return client;
    }
}

internal record GroupStudentDto
{
    public Guid Id { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public bool IsActive { get; init; }
}

internal record GroupAnalyticsDataDto
{
    public int CompletedTests { get; init; }
    public double AverageScore { get; init; }
    public double PassRate { get; init; }
    public List<TopPerformerDto> TopPerformers { get; init; } = [];
    public GroupProgressDto? GroupProgress { get; init; }
}
