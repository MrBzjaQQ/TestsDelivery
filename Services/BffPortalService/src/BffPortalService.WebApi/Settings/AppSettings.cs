using BffPortalService.Infrastructure.HttpClients.External.HttpClientFactories;

namespace BffPortalService.WebApi.Settings;

public sealed record AppSettings
{
    public required string ConnectionString { get; init; }
    public string AppName { get; init; } = "bff-portal-service:test";
    public ServiceUrls ServiceUrls { get; init; } = new();
    public JwtSettings Jwt { get; init; } = new();
    public CacheSettings Cache { get; init; } = new();
    public HealthCheckSettings HealthChecks { get; init; } = new();
}

public sealed record JwtSettings
{
    public string SecretKey { get; init; } = "bff-portal-secret";
    public string Issuer { get; init; } = "TestsDelivery";
    public string Audience { get; init; } = "TestsDelivery";
    public int ExpirationMinutes { get; init; } = 60;
}

public sealed record CacheSettings
{
    public int StudentProfileDurationMinutes { get; init; } = 5;
    public int AvailableTestsDurationMinutes { get; init; } = 10;
    public int GroupAnalyticsDurationMinutes { get; init; } = 15;
    public int TestQuestionsDurationMinutes { get; init; } = 60;
}

public sealed record HealthCheckSettings
{
    public bool Enabled { get; init; } = true;
    public int DatabaseTimeoutSeconds { get; init; } = 5;
    public int ExternalServiceTimeoutSeconds { get; init; } = 3;
}
