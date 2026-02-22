using BffPortalService.Application.Contracts;
using BffPortalService.Application.DTOs.Responses;
using BffPortalService.Application.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BffPortalService.Tests.Unit.Services;

public class GroupAnalyticsServiceTests
{
    private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly Mock<ILogger<GroupAnalyticsService>> _loggerMock;
    private readonly GroupAnalyticsService _service;

    public GroupAnalyticsServiceTests()
    {
        _httpClientFactoryMock = new Mock<IHttpClientFactory>();
        _cacheServiceMock = new Mock<ICacheService>();
        _loggerMock = new Mock<ILogger<GroupAnalyticsService>>();
        _service = new GroupAnalyticsService(
            _httpClientFactoryMock.Object,
            _cacheServiceMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public void Constructor_ShouldInitialize()
    {
        _service.Should().NotBeNull();
    }

    [Fact]
    public async Task InvalidateGroupCacheAsync_ShouldRemoveCache()
    {
        var groupId = Guid.NewGuid();

        await _service.InvalidateGroupCacheAsync(groupId, CancellationToken.None);

        _cacheServiceMock.Verify(
            x => x.RemoveAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetGroupAnalyticsAsync_WhenCacheExists_ShouldReturnCachedData()
    {
        var groupId = Guid.NewGuid();
        var accessToken = "test-token";
        var cachedAnalytics = new GroupAnalyticsDto
        {
            GroupId = groupId,
            GroupName = "Cached Group",
            TotalStudents = 25
        };

        _cacheServiceMock
            .Setup(x => x.GetOrAddAsync(
                It.IsAny<string>(),
                It.IsAny<Func<CancellationToken, Task<GroupAnalyticsDto>>>(),
                It.IsAny<TimeSpan>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(cachedAnalytics);

        var result = await _service.GetGroupAnalyticsAsync(groupId, accessToken, CancellationToken.None);

        result.Should().Be(cachedAnalytics);
    }

    [Fact]
    public async Task GetGroupAnalyticsAsync_ShouldCallCacheWithCorrectKey()
    {
        var groupId = Guid.NewGuid();
        var accessToken = "test-token";

        _cacheServiceMock
            .Setup(x => x.GetOrAddAsync(
                It.IsAny<string>(),
                It.IsAny<Func<CancellationToken, Task<GroupAnalyticsDto>>>(),
                It.IsAny<TimeSpan>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GroupAnalyticsDto { GroupId = groupId });

        await _service.GetGroupAnalyticsAsync(groupId, accessToken, CancellationToken.None);

        _cacheServiceMock.Verify(
            x => x.GetOrAddAsync(
                It.Is<string>(key => key.Contains(groupId.ToString())),
                It.IsAny<Func<CancellationToken, Task<GroupAnalyticsDto>>>(),
                It.IsAny<TimeSpan>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task InvalidateGroupCacheAsync_ShouldUseCorrectCacheKey()
    {
        var groupId = Guid.NewGuid();

        await _service.InvalidateGroupCacheAsync(groupId, CancellationToken.None);

        _cacheServiceMock.Verify(
            x => x.RemoveAsync(
                It.Is<string>(key => key.Contains(groupId.ToString())),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
