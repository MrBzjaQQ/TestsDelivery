using System.Net.Http.Headers;
using System.Net.Http.Json;
using BffPortalService.Application.Contracts;
using BffPortalService.Application.DTOs.Responses;
using BffPortalService.Domain.Exceptions;
using BffPortalService.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace BffPortalService.Application.Services;

public class StudentPortalService : IStudentPortalService
{
    private const string StudentServiceClientName = "StudentService";
    private const string TestCheckingServiceClientName = "TestCheckingService";

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ICacheService _cacheService;
    private readonly ILogger<StudentPortalService> _logger;

    public StudentPortalService(
        IHttpClientFactory httpClientFactory,
        ICacheService cacheService,
        ILogger<StudentPortalService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task<StudentProfileDto> GetStudentProfileAsync(Guid studentId, string accessToken, CancellationToken ct)
    {
        var cacheKey = PortalDataKey.StudentProfile(studentId).ToString();

        return await _cacheService.GetOrAddAsync(
            cacheKey,
            async _ => await FetchStudentProfileAsync(studentId, accessToken, ct),
            CacheDuration.StudentProfile.Value,
            ct);
    }

    private async Task<StudentProfileDto> FetchStudentProfileAsync(Guid studentId, string accessToken, CancellationToken ct)
    {
        try
        {
            var studentClient = CreateClient(StudentServiceClientName, accessToken);
            var testClient = CreateClient(TestCheckingServiceClientName, accessToken);

            var studentTask = studentClient.GetAsync($"api/v1/students/{studentId}", ct);
            var testResultsTask = testClient.GetAsync($"api/v1/students/{studentId}/results", ct);

            await Task.WhenAll(studentTask, testResultsTask);

            var studentResponse = studentTask.Result;
            var testResultsResponse = testResultsTask.Result;

            if (!studentResponse.IsSuccessStatusCode)
            {
                if (studentResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new PortalDataNotFoundException("StudentProfile", studentId.ToString());
                }

                _logger.LogError("Failed to fetch student profile. Status: {StatusCode}", studentResponse.StatusCode);
                throw new ServiceUnavailableException("StudentService", "Failed to retrieve student profile");
            }

            var profile = await studentResponse.Content.ReadFromJsonAsync<StudentProfileDto>(ct);

            if (testResultsResponse.IsSuccessStatusCode)
            {
                var results = await testResultsResponse.Content.ReadFromJsonAsync<List<RecentTestDto>>(ct);
                if (results is not null && profile is not null)
                {
                    profile = profile with { RecentTests = results };
                }
            }

            return profile ?? throw new PortalDataNotFoundException("StudentProfile", studentId.ToString());
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error while fetching student profile for {StudentId}", studentId);
            throw new ServiceUnavailableException("StudentService", "Student service is temporarily unavailable", ex);
        }
    }

    public async Task InvalidateStudentCacheAsync(Guid studentId, CancellationToken ct)
    {
        var cacheKey = PortalDataKey.StudentProfile(studentId).ToString();
        await _cacheService.RemoveAsync(cacheKey, ct);
        _logger.LogInformation("Cache invalidated for student {StudentId}", studentId);
    }

    private HttpClient CreateClient(string clientName, string accessToken)
    {
        var client = _httpClientFactory.CreateClient(clientName);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        return client;
    }
}
