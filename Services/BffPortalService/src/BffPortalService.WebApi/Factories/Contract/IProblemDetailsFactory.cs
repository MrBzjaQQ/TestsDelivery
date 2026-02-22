using Microsoft.AspNetCore.Mvc;

namespace BffPortalService.WebApi.Factories.Contract;

public interface IProblemDetailsFactory
{
    ProblemDetails GetProblemDetails(HttpContext httpContext, Exception exception);
}
