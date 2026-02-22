namespace IdentityService.Application.Settings;

public class IdentitySettings
{
    public int PasswordRequiredLength { get; set; } = 8;

    public bool PasswordRequireDigit { get; set; } = true;

    public bool PasswordRequireLowercase { get; set; } = true;

    public bool PasswordRequireUppercase { get; set; } = true;

    public bool PasswordRequireNonAlphanumeric { get; set; } = true;

    public bool UserRequireUniqueEmail { get; set; } = true;

    public bool EmailConfirmationRequired { get; set; } = true;
}
