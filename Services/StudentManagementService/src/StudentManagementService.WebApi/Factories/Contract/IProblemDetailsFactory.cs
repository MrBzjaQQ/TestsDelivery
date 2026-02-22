using Microsoft.AspNetCore.Mvc;

namespace StudentManagementService.WebApi.Factories.Contract;

public interface IProblemDetailsFactory
{
    ProblemDetails GetProblemDetails(HttpContext httpContext, Exception exception);
}
