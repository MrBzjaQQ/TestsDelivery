using System.Diagnostics;
using BffPortalService.WebApi.Wrappers.Contract;

namespace BffPortalService.WebApi.Wrappers;

public class ActivityWrapper : IActivityWrapper
{
    private readonly Activity? _activity;

    public ActivityWrapper()
    {
        _activity = Activity.Current;
    }

    public string? Id => _activity?.Id;
}
