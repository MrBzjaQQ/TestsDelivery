namespace TestCheckingService.Application.Settings;

public class ScoringSettings
{
    public int PassPercentage { get; init; } = 70;

    public bool AllowRetries { get; init; } = true;

    public int MaxAttempts { get; init; } = 3;

    public int AttemptCooldownMinutes { get; init; } = 5;
}
