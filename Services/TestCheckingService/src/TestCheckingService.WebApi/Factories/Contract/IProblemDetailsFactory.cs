using Microsoft.AspNetCore.Mvc;

namespace TestCheckingService.WebApi.Factories.Contract;

public interface IProblemDetailsFactory
{
    ProblemDetails GetProblemDetails(HttpContext httpContext, Exception exception);
}
