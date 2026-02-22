using System.Diagnostics;
using NotificationService.WebApi.Wrappers.Contract;

namespace NotificationService.WebApi.Wrappers;

public class ActivityWrapper : IActivityWrapper
{
    private readonly Activity? _activity;

    public ActivityWrapper()
    {
        _activity = Activity.Current;
    }

    public string? Id => _activity?.Id;
}
