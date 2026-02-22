using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using FluentAssertions;
using IdentityService.Application.DTOs.Requests;
using IdentityService.Domain.ValueObjects;
using IdentityService.Infrastructure.Database.Context;
using IdentityService.Infrastructure.Database.Identity;
using IdentityService.Infrastructure.Database.Services;
using IdentityService.WebApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using Testcontainers.PostgreSql;
using Xunit;

namespace IdentityService.Tests.Integration.Controllers;

public class AuthControllerTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:16")
        .WithDatabase("testsdelivery_identity_test")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    private HttpClient _httpClient = null!;
    private WebApplicationFactory<Program> _factory = null!;

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Testing");
                builder.ConfigureAppConfiguration((context, config) =>
                {
                    config.AddInMemoryCollection(new Dictionary<string, string?>
                    {
                        ["ConnectionStrings:DefaultConnection"] = _dbContainer.GetConnectionString(),
                        ["Jwt:SecretKey"] = "test-secret-key-for-integration-tests-min-32-chars",
                        ["Jwt:Issuer"] = "TestsDelivery",
                        ["Jwt:Audience"] = "TestsDelivery",
                        ["Jwt:ExpirationMinutes"] = "60",
                        ["Jwt:RefreshTokenExpirationHours"] = "24",
                        ["Identity:PasswordRequiredLength"] = "8",
                        ["Identity:PasswordRequireDigit"] = "true",
                        ["Identity:PasswordRequireLowercase"] = "true",
                        ["Identity:PasswordRequireUppercase"] = "true",
                        ["Identity:PasswordRequireNonAlphanumeric"] = "true",
                        ["Identity:UserRequireUniqueEmail"] = "true",
                        ["Identity:EmailConfirmationRequired"] = "false",
                        ["RabbitMQ:Host"] = "localhost",
                        ["RabbitMQ:Port"] = "5672",
                        ["RabbitMQ:Username"] = "guest",
                        ["RabbitMQ:Password"] = "guest",
                        ["RabbitMQ:VirtualHost"] = "/",
                    });
                });
            });

        _httpClient = _factory.CreateClient();

        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
        await dbContext.Database.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
        _httpClient.Dispose();
        await _factory.DisposeAsync();
    }

    [Fact]
    public async Task Register_ShouldReturn201_WhenValidRequest()
    {
        var request = new RegisterRequest
        {
            Email = "test@example.com",
            Password = "Password123!",
            FirstName = "John",
            LastName = "Doe",
            Role = UserRole.Student,
        };

        var response = await _httpClient.PostAsJsonAsync("/api/v1/auth/register", request);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
    }

    [Fact]
    public async Task Register_ShouldReturn409_WhenEmailExists()
    {
        var request = new RegisterRequest
        {
            Email = "duplicate@example.com",
            Password = "Password123!",
            FirstName = "John",
            LastName = "Doe",
            Role = UserRole.Student,
        };

        await _httpClient.PostAsJsonAsync("/api/v1/auth/register", request);

        var response = await _httpClient.PostAsJsonAsync("/api/v1/auth/register", request);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Login_ShouldReturn200_WhenValidCredentials()
    {
        var registerRequest = new RegisterRequest
        {
            Email = "login@example.com",
            Password = "Password123!",
            FirstName = "John",
            LastName = "Doe",
            Role = UserRole.Student,
        };

        await _httpClient.PostAsJsonAsync("/api/v1/auth/register", registerRequest);

        using var scope = _factory.Services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var user = await userManager.FindByEmailAsync("login@example.com");
        if (user != null)
        {
            user.EmailVerified = true;
            await userManager.UpdateAsync(user);
        }

        var loginRequest = new LoginRequest
        {
            Email = "login@example.com",
            Password = "Password123!",
        };

        var response = await _httpClient.PostAsJsonAsync("/api/v1/auth/login", loginRequest);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Fact]
    public async Task Login_ShouldReturn400_WhenInvalidCredentials()
    {
        var loginRequest = new LoginRequest
        {
            Email = "nonexistent@example.com",
            Password = "WrongPassword123!",
        };

        var response = await _httpClient.PostAsJsonAsync("/api/v1/auth/login", loginRequest);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }
}
