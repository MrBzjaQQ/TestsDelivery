using Microsoft.EntityFrameworkCore;
using StudentManagementService.Infrastructure.Database.Context;
using StudentManagementService.Tests.Integration.TestInfrastructure;
using Xunit;

namespace StudentManagementService.Tests.Integration.TestInfrastructure;

[CollectionDefinition(nameof(DatabaseCollection))]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
{
}

[Collection(nameof(DatabaseCollection))]
public abstract class DbTestsBase : IAsyncLifetime
{
    private readonly DatabaseFixture _fixture;
    protected StudentDbContext DbContext = null!;

    protected DbTestsBase(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    public async Task InitializeAsync()
    {
        var options = new DbContextOptionsBuilder<StudentDbContext>()
            .UseNpgsql(_fixture.ConnectionString)
            .Options;

        DbContext = new StudentDbContext(options);
        await DbContext.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await DbContext.DisposeAsync();
    }
}
