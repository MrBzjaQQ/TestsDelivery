using System.Net.Http.Headers;
using System.Net.Http.Json;
using BffPortalService.Application.Contracts;
using BffPortalService.Application.DTOs.Requests;
using BffPortalService.Application.DTOs.Responses;
using BffPortalService.Application.Exceptions;
using BffPortalService.Domain.Exceptions;
using BffPortalService.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace BffPortalService.Application.Services;

public class TestsPortalService : ITestsPortalService
{
    private const string QuestionServiceClientName = "QuestionService";
    private const string StudentServiceClientName = "StudentService";
    private const string TestCheckingServiceClientName = "TestCheckingService";

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ICacheService _cacheService;
    private readonly ILogger<TestsPortalService> _logger;

    public TestsPortalService(
        IHttpClientFactory httpClientFactory,
        ICacheService cacheService,
        ILogger<TestsPortalService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task<AvailableTestsDto> GetAvailableTestsAsync(Guid userId, string accessToken, GetTestsRequest request, CancellationToken ct)
    {
        var cacheKey = $"{PortalDataKey.AvailableTests(userId)}:{request.GroupId}:{request.Status}";

        return await _cacheService.GetOrAddAsync(
            cacheKey,
            async _ => await FetchAvailableTestsAsync(userId, accessToken, request, ct),
            CacheDuration.AvailableTests.Value,
            ct);
    }

    private async Task<AvailableTestsDto> FetchAvailableTestsAsync(Guid userId, string accessToken, GetTestsRequest request, CancellationToken ct)
    {
        try
        {
            var client = CreateClient(TestCheckingServiceClientName, accessToken);

            var url = $"api/v1/tests/available?userId={userId}";
            if (request.GroupId.HasValue)
            {
                url += $"&groupId={request.GroupId.Value}";
            }

            if (!string.IsNullOrEmpty(request.Status))
            {
                url += $"&status={request.Status}";
            }

            var response = await client.GetAsync(url, ct);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to fetch available tests. Status: {StatusCode}", response.StatusCode);
                throw new ServiceUnavailableException("TestCheckingService", "Failed to retrieve available tests");
            }

            var tests = await response.Content.ReadFromJsonAsync<AvailableTestsDto>(ct);
            return tests ?? new AvailableTestsDto();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error while fetching available tests for user {UserId}", userId);
            throw new ServiceUnavailableException("TestCheckingService", "Test checking service is temporarily unavailable", ex);
        }
    }

    public async Task<StartTestResponseDto> StartTestAsync(Guid testId, Guid studentId, string accessToken, CancellationToken ct)
    {
        try
        {
            var client = CreateClient(TestCheckingServiceClientName, accessToken);

            var response = await client.PostAsJsonAsync($"api/v1/tests/{testId}/start", new { studentId }, ct);

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new PortalDataNotFoundException("Test", testId.ToString());
                }

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var error = await response.Content.ReadAsStringAsync(ct);
                    throw new DataAggregationException("TestCheckingService", "StartTest", error);
                }

                _logger.LogError("Failed to start test. Status: {StatusCode}", response.StatusCode);
                throw new ServiceUnavailableException("TestCheckingService", "Failed to start test");
            }

            var result = await response.Content.ReadFromJsonAsync<StartTestResponseDto>(ct);

            await InvalidateTestsCacheAsync(studentId, ct);

            return result ?? throw new DataAggregationException("TestCheckingService", "StartTest", "Empty response received");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error while starting test {TestId}", testId);
            throw new ServiceUnavailableException("TestCheckingService", "Test checking service is temporarily unavailable", ex);
        }
    }

    public async Task<TestQuestionsDto> GetTestQuestionsAsync(Guid testId, string accessToken, CancellationToken ct)
    {
        var cacheKey = PortalDataKey.TestQuestions(testId).ToString();

        return await _cacheService.GetOrAddAsync(
            cacheKey,
            async _ => await FetchTestQuestionsAsync(testId, accessToken, ct),
            CacheDuration.TestQuestions.Value,
            ct);
    }

    private async Task<TestQuestionsDto> FetchTestQuestionsAsync(Guid testId, string accessToken, CancellationToken ct)
    {
        try
        {
            var client = CreateClient(QuestionServiceClientName, accessToken);

            var response = await client.GetAsync($"api/v1/tests/{testId}/questions", ct);

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new PortalDataNotFoundException("TestQuestions", testId.ToString());
                }

                _logger.LogError("Failed to fetch test questions. Status: {StatusCode}", response.StatusCode);
                throw new ServiceUnavailableException("QuestionService", "Failed to retrieve test questions");
            }

            var questions = await response.Content.ReadFromJsonAsync<TestQuestionsDto>(ct);
            return questions ?? throw new PortalDataNotFoundException("TestQuestions", testId.ToString());
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error while fetching test questions for test {TestId}", testId);
            throw new ServiceUnavailableException("QuestionService", "Question service is temporarily unavailable", ex);
        }
    }

    public async Task<SubmitTestResponseDto> SubmitTestAnswersAsync(Guid testId, Guid studentId, string accessToken, SubmitTestRequest request, CancellationToken ct)
    {
        try
        {
            var client = CreateClient(TestCheckingServiceClientName, accessToken);

            var payload = new
            {
                studentId,
                answers = request.Answers
            };

            var response = await client.PostAsJsonAsync($"api/v1/tests/{testId}/submit", payload, ct);

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new PortalDataNotFoundException("Test", testId.ToString());
                }

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var error = await response.Content.ReadAsStringAsync(ct);
                    throw new DataAggregationException("TestCheckingService", "SubmitTest", error);
                }

                _logger.LogError("Failed to submit test. Status: {StatusCode}", response.StatusCode);
                throw new ServiceUnavailableException("TestCheckingService", "Failed to submit test");
            }

            var result = await response.Content.ReadFromJsonAsync<SubmitTestResponseDto>(ct);

            await InvalidateTestsCacheAsync(studentId, ct);
            var profileCacheKey = PortalDataKey.StudentProfile(studentId).ToString();
            await _cacheService.RemoveAsync(profileCacheKey, ct);

            return result ?? throw new DataAggregationException("TestCheckingService", "SubmitTest", "Empty response received");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error while submitting test {TestId}", testId);
            throw new ServiceUnavailableException("TestCheckingService", "Test checking service is temporarily unavailable", ex);
        }
    }

    public async Task<TestResultsDto> GetTestResultsAsync(Guid testId, Guid studentId, string accessToken, CancellationToken ct)
    {
        try
        {
            var client = CreateClient(TestCheckingServiceClientName, accessToken);

            var response = await client.GetAsync($"api/v1/tests/{testId}/results?studentId={studentId}", ct);

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new PortalDataNotFoundException("TestResults", $"{testId}:{studentId}");
                }

                _logger.LogError("Failed to fetch test results. Status: {StatusCode}", response.StatusCode);
                throw new ServiceUnavailableException("TestCheckingService", "Failed to retrieve test results");
            }

            var results = await response.Content.ReadFromJsonAsync<TestResultsDto>(ct);
            return results ?? throw new PortalDataNotFoundException("TestResults", $"{testId}:{studentId}");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error while fetching test results for test {TestId}", testId);
            throw new ServiceUnavailableException("TestCheckingService", "Test checking service is temporarily unavailable", ex);
        }
    }

    public async Task InvalidateTestsCacheAsync(Guid userId, CancellationToken ct)
    {
        var cacheKeyPrefix = PortalDataKey.AvailableTests(userId).ToString();
        await _cacheService.RemoveByPrefixAsync(cacheKeyPrefix, ct);
        _logger.LogInformation("Cache invalidated for user tests {UserId}", userId);
    }

    private HttpClient CreateClient(string clientName, string accessToken)
    {
        var client = _httpClientFactory.CreateClient(clientName);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        return client;
    }
}
