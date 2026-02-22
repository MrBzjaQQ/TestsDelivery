namespace BffPortalService.Domain.Entities;

public class PortalSettings
{
    public Guid Id { get; init; }

    public string SettingKey { get; init; } = string.Empty;

    public string SettingValue { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public DateTime UpdatedAt { get; init; }
}
