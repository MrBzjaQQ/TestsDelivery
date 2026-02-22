using System.Diagnostics;
using FileStorageService.WebApi.Wrappers.Contract;

namespace FileStorageService.WebApi.Wrappers;

public class ActivityWrapper : IActivityWrapper
{
    private readonly Activity? _activity;

    public ActivityWrapper()
    {
        _activity = Activity.Current;
    }

    public string? Id => _activity?.Id;
}
