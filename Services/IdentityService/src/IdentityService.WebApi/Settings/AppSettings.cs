namespace IdentityService.WebApi.Settings;

public sealed record AppSettings
{
    public required string ConnectionString { get; init; }

    public required string AppName { get; init; } = "identity-service:test";
}
