namespace TestCheckingService.WebApi.Settings;

public sealed record AppSettings
{
    public required string ConnectionString { get; init; }

    public string AppName { get; init; } = "test-checking-service:test";

    public RabbitMQSettings RabbitMQ { get; init; } = new();

    public ScoringSettings Scoring { get; init; } = new();
}

public sealed record RabbitMQSettings
{
    public string Host { get; init; } = "localhost";

    public int Port { get; init; } = 5672;

    public string Username { get; init; } = "guest";

    public string Password { get; init; } = "guest";

    public string VirtualHost { get; init; } = "/";
}

public sealed record ScoringSettings
{
    public int PassPercentage { get; init; } = 70;

    public bool AllowRetries { get; init; } = true;

    public int MaxAttempts { get; init; } = 3;

    public int AttemptCooldownMinutes { get; init; } = 5;
}
