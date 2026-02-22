using Microsoft.AspNetCore.Mvc;

namespace QuestionManagementService.WebApi.Factories.Contract;

public interface IProblemDetailsFactory
{
    ProblemDetails GetProblemDetails(HttpContext httpContext, Exception exception);
}
