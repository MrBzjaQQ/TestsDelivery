using CommunityToolkit.Diagnostics;
using BffPortalService.WebApi.Constants;
using BffPortalService.WebApi.Factories.Contract;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BffPortalService.WebApi.Handler;

public class CustomExceptionHandler : IExceptionHandler
{
    private readonly ILogger<CustomExceptionHandler> _logger;
    private readonly IProblemDetailsService _problemDetailsService;
    private readonly IProblemDetailsFactory _problemDetailsFactory;

    public CustomExceptionHandler(
        ILogger<CustomExceptionHandler> logger,
        IProblemDetailsService problemDetailsService,
        IProblemDetailsFactory problemDetailsFactory)
    {
        _logger = logger;
        _problemDetailsService = problemDetailsService;
        _problemDetailsFactory = problemDetailsFactory;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var problemDetails = _problemDetailsFactory.GetProblemDetails(httpContext, exception);

        Guard.IsNotNull(problemDetails);

        object? traceId;
        problemDetails.Extensions.TryGetValue(ProblemDetailsConstants.TraceIdFieldName, out traceId);

        _logger.LogError(exception, LogMessageConstants.ExceptionOccuredErrorMessageTemplate, problemDetails.Instance, traceId);

        return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            Exception = exception,
            HttpContext = httpContext,
            ProblemDetails = problemDetails
        });
    }
}
