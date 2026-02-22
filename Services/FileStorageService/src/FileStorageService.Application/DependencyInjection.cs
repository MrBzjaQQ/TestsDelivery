using FileStorageService.Application.Contracts;
using FileStorageService.Application.Services;
using FileStorageService.Application.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FileStorageService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IFileUploadService, FileUploadService>();
        services.AddScoped<IFileDownloadService, FileDownloadService>();
        services.AddScoped<IFileDeleteService, FileDeleteService>();
        services.AddSingleton<IImageProcessor, ImageProcessor>();
        services.AddSingleton<IFileValidator, FileValidator>();

 services.AddOptions<FileStorageSettings>()
            .Configure<IConfiguration>((settings, configuration) =>
            {
                configuration.GetSection("FileStorage").Bind(settings);
            });

        return services;
    }
}
