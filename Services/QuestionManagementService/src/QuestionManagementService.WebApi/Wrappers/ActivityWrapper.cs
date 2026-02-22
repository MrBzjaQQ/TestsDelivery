using System.Diagnostics;
using QuestionManagementService.WebApi.Wrappers.Contract;

namespace QuestionManagementService.WebApi.Wrappers;

public class ActivityWrapper : IActivityWrapper
{
    private readonly Activity? _activity;

    public ActivityWrapper()
    {
        _activity = Activity.Current;
    }

    public string? Id => _activity?.Id;
}
