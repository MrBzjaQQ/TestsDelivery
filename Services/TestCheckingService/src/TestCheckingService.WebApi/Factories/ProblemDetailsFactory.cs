using CommunityToolkit.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TestCheckingService.WebApi.Constants;
using TestCheckingService.WebApi.Factories.Contract;
using TestCheckingService.WebApi.Wrappers.Contract;

namespace TestCheckingService.WebApi.Factories;

public class ProblemDetailsFactory : IProblemDetailsFactory
{
    private readonly IActivityWrapperFactory _activityWrapperFactory;

    public ProblemDetailsFactory(IActivityWrapperFactory activityWrapperFactory)
    {
        _activityWrapperFactory = activityWrapperFactory;
    }

    public ProblemDetails GetProblemDetails(HttpContext httpContext, Exception exception)
    {
        Guard.IsNotNull(httpContext);
        Guard.IsNotNull(exception);

        IActivityWrapper? activity = _activityWrapperFactory.GetActivity();

        var problemDetails = new ProblemDetails
        {
            Title = ProblemDetailsConstants.ErrorTitle,
            Type = exception.GetType().Name,
            Detail = exception.Message,
            Instance = $"{httpContext.Request.Method}: {httpContext.Request.Path}"
        };

        problemDetails.Extensions.TryAdd(ProblemDetailsConstants.RequestIdFieldName, httpContext.TraceIdentifier);
        problemDetails.Extensions.TryAdd(ProblemDetailsConstants.TraceIdFieldName, activity?.Id);

        return problemDetails;
    }
}
