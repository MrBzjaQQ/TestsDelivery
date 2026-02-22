namespace TestCheckingService.WebApi.Wrappers;

using TestCheckingService.WebApi.Wrappers.Contract;

public class ActivityWrapper : IActivityWrapper
{
    public string? Id => System.Diagnostics.Activity.Current?.Id;
}
