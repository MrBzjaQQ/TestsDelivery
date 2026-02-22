namespace StudentManagementService.WebApi.Settings;

public sealed record AppSettings
{
    public required string ConnectionString { get; init; }
}
