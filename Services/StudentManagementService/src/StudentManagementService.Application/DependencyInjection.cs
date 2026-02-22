using Microsoft.Extensions.DependencyInjection;
using StudentManagementService.Application.Contracts;
using StudentManagementService.Application.Services;

namespace StudentManagementService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<ITestAssignmentService, TestAssignmentService>();
        services.AddScoped<IProgressTrackingService, ProgressTrackingService>();
        services.AddScoped<IGroupService, GroupService>();

        return services;
    }
}
