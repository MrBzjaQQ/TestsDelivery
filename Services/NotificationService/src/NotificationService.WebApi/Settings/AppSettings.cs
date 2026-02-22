namespace NotificationService.WebApi.Settings;

public sealed record AppSettings
{
    public required string ConnectionString { get; init; }

    public SmtpSettings Smtp { get; init; } = new();
}

public sealed record SmtpSettings
{
    public string Host { get; init; } = "localhost";

    public int Port { get; init; } = 587;

    public string Username { get; init; } = string.Empty;

    public string Password { get; init; } = string.Empty;

    public string From { get; init; } = "noreply@testsdelivery.com";

    public string DisplayName { get; init; } = "TestsDelivery";

    public bool EnableSsl { get; init; } = true;
}
