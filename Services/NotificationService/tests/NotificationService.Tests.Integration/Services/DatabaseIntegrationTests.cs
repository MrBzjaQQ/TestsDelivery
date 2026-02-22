using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NotificationService.Tests.Integration.TestInfrastructure;
using Xunit;

namespace NotificationService.Tests.Integration;

public class DatabaseIntegrationTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;

    public DatabaseIntegrationTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Database_Should_BeAbleToConnect()
    {
        await using var context = await _fixture.CreateDbContextAsync();

        var canConnect = await context.Database.CanConnectAsync();

        canConnect.Should().BeTrue();
    }

    [Fact]
    public async Task Database_Should_HaveNotificationsTable()
    {
        await using var context = await _fixture.CreateDbContextAsync();

        var notifications = await context.Notifications.ToListAsync();

        notifications.Should().NotBeNull();
    }
}
