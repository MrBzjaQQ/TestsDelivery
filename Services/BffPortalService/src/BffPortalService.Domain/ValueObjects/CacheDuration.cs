namespace BffPortalService.Domain.ValueObjects;

public record CacheDuration
{
    public TimeSpan Value { get; }

    private CacheDuration(TimeSpan value)
    {
        Value = value;
    }

    public static CacheDuration StudentProfile => new(TimeSpan.FromMinutes(5));

    public static CacheDuration AvailableTests => new(TimeSpan.FromMinutes(10));

    public static CacheDuration GroupAnalytics => new(TimeSpan.FromMinutes(15));

    public static CacheDuration TestQuestions => new(TimeSpan.FromHours(1));

    public static CacheDuration FromMinutes(int minutes) =>
        new(TimeSpan.FromMinutes(minutes));
}
