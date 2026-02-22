using BffPortalService.Infrastructure.HttpClients.External.HttpClientFactories;
using BffPortalService.WebApi.Settings;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace BffPortalService.WebApi.HealthChecks;

public class ExternalServiceHealthCheck : IHealthCheck
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<ExternalServiceHealthCheck> _logger;
    private readonly ServiceUrls _serviceUrls;

    public ExternalServiceHealthCheck(
        IHttpClientFactory httpClientFactory,
        ILogger<ExternalServiceHealthCheck> logger,
        IOptions<AppSettings> appSettings)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _serviceUrls = appSettings.Value.ServiceUrls;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var services = new Dictionary<string, string>
        {
            ["Identity"] = _serviceUrls.IdentityServiceUrl,
            ["Question"] = _serviceUrls.QuestionServiceUrl,
            ["Student"] = _serviceUrls.StudentServiceUrl,
            ["TestChecking"] = _serviceUrls.TestCheckingServiceUrl
        };

        var healthyServices = new List<string>();
        var unhealthyServices = new Dictionary<string, string>();

        foreach (var service in services)
        {
            try
            {
                var client = _httpClientFactory.CreateClient($"{service.Key}Service");
                var response = await client.GetAsync("/quickhealth", cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    healthyServices.Add(service.Key);
                }
                else
                {
                    unhealthyServices[service.Key] = $"Status: {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                unhealthyServices[service.Key] = ex.Message;
                _logger.LogWarning(ex, "Health check failed for {ServiceName}", service.Key);
            }
        }

        var data = new Dictionary<string, object>
        {
            ["HealthyServices"] = string.Join(", ", healthyServices),
            ["UnhealthyServices"] = string.Join(", ", unhealthyServices.Keys)
        };

        if (unhealthyServices.Count == 0)
        {
            return HealthCheckResult.Healthy("All external services healthy", data);
        }

        var message = $"Some services unhealthy: {string.Join(", ", unhealthyServices.Select(x => $"{x.Key}: {x.Value}"))}";
        return HealthCheckResult.Unhealthy(message, data: data);
    }
}
