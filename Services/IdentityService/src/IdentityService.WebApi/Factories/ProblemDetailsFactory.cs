using System.Diagnostics;
using IdentityService.WebApi.Constants;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.WebApi.Factories;

public interface IProblemDetailsFactory
{
    ProblemDetails CreateProblemDetails(
        HttpContext httpContext,
        int? statusCode = null,
        string? title = null,
        string? type = null,
        string? detail = null,
        string? instance = null);
}

public class ProblemDetailsFactory : IProblemDetailsFactory
{
    public ProblemDetails CreateProblemDetails(
        HttpContext httpContext,
        int? statusCode = null,
        string? title = null,
        string? type = null,
        string? detail = null,
        string? instance = null)
    {
        var problemDetails = new ProblemDetails
        {
            Status = statusCode ?? 500,
            Title = title ?? ProblemDetailsConstants.ErrorTitle,
            Type = type,
            Detail = detail,
            Instance = instance ?? $"{httpContext.Request.Method}: {httpContext.Request.Path}",
        };

        var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;
        problemDetails.Extensions[ProblemDetailsConstants.TraceIdFieldName] = traceId;

        return problemDetails;
    }
}
