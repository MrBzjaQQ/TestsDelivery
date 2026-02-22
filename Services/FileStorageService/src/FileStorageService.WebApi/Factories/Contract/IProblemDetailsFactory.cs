using Microsoft.AspNetCore.Mvc;

namespace FileStorageService.WebApi.Factories.Contract;

public interface IProblemDetailsFactory
{
    ProblemDetails GetProblemDetails(HttpContext httpContext, Exception exception);
}
