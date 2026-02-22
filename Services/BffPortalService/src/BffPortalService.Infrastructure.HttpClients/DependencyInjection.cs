using BffPortalService.Infrastructure.HttpClients.External.Clients;
using BffPortalService.Infrastructure.HttpClients.External.HttpClientFactories;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace BffPortalService.Infrastructure.HttpClients;

public static class DependencyInjection
{
    public static IServiceCollection AddHttpClients(this IServiceCollection services, ServiceUrls serviceUrls)
    {
        services.AddHttpClient("QuestionService", client =>
        {
            client.BaseAddress = new Uri(serviceUrls.QuestionServiceUrl);
            client.Timeout = TimeSpan.FromSeconds(30);
        });

        services.AddHttpClient("StudentService", client =>
        {
            client.BaseAddress = new Uri(serviceUrls.StudentServiceUrl);
            client.Timeout = TimeSpan.FromSeconds(30);
        });

        services.AddHttpClient("TestCheckingService", client =>
        {
            client.BaseAddress = new Uri(serviceUrls.TestCheckingServiceUrl);
            client.Timeout = TimeSpan.FromSeconds(30);
        });

        services.AddHttpClient("IdentityService", client =>
        {
            client.BaseAddress = new Uri(serviceUrls.IdentityServiceUrl);
            client.Timeout = TimeSpan.FromSeconds(30);
        });

        services.AddRefitClient<IIdentityServiceClient>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(serviceUrls.IdentityServiceUrl));

        services.AddRefitClient<IQuestionServiceClient>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(serviceUrls.QuestionServiceUrl));

        services.AddRefitClient<IStudentServiceClient>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(serviceUrls.StudentServiceUrl));

        services.AddRefitClient<ITestCheckingServiceClient>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(serviceUrls.TestCheckingServiceUrl));

        return services;
    }
}
