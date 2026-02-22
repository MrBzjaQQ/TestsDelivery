using System.Diagnostics;
using StudentManagementService.WebApi.Wrappers.Contract;

namespace StudentManagementService.WebApi.Wrappers;

public class ActivityWrapper : IActivityWrapper
{
    private readonly Activity? _activity;

    public ActivityWrapper()
    {
        _activity = Activity.Current;
    }

    public string? Id => _activity?.Id;
}
