using System.Net;
using IdentityService.Domain.Exceptions;
using IdentityService.WebApi.Constants;
using IdentityService.WebApi.Factories;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.WebApi.Handler;

public class CustomExceptionHandler : IExceptionHandler
{
    private readonly IProblemDetailsFactory _problemDetailsFactory;
    private readonly ILogger<CustomExceptionHandler> _logger;

    public CustomExceptionHandler(
        IProblemDetailsFactory problemDetailsFactory,
        ILogger<CustomExceptionHandler> logger)
    {
        _problemDetailsFactory = problemDetailsFactory;
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "An unhandled exception occurred");

        var (statusCode, type, detail) = exception switch
        {
            UserNotFoundException ex => ((int)HttpStatusCode.NotFound, "UserNotFoundException", ex.Message),
            InvalidCredentialsException ex => ((int)HttpStatusCode.BadRequest, "InvalidCredentialsException", ex.Message),
            DuplicateEmailException ex => ((int)HttpStatusCode.Conflict, "DuplicateEmailException", ex.Message),
            EmailNotVerifiedException ex => ((int)HttpStatusCode.Forbidden, "EmailNotVerifiedException", ex.Message),
            TokenRefreshException ex => ((int)HttpStatusCode.Unauthorized, "TokenRefreshException", ex.Message),
            AccountLockedException ex => ((int)HttpStatusCode.Forbidden, "AccountLockedException", ex.Message),
            ArgumentException ex => ((int)HttpStatusCode.BadRequest, "ArgumentException", ex.Message),
            _ => ((int)HttpStatusCode.InternalServerError, "InternalServerError", "An unexpected error occurred"),
        };

        var problemDetails = _problemDetailsFactory.CreateProblemDetails(
            httpContext,
            statusCode,
            ProblemDetailsConstants.ErrorTitle,
            type,
            detail);

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
