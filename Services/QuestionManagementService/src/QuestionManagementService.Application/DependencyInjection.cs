using Microsoft.Extensions.DependencyInjection;
using QuestionManagementService.Application.Contracts;
using QuestionManagementService.Application.Services;

namespace QuestionManagementService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IQuestionService, QuestionService>();
        services.AddScoped<IQuestionBankService, QuestionBankService>();
        services.AddScoped<ITestService, TestService>();
        services.AddScoped<ITestTemplateService, TestTemplateService>();

        return services;
    }
}
