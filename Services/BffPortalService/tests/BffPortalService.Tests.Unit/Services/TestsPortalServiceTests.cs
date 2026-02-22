using BffPortalService.Application.Contracts;
using BffPortalService.Application.DTOs.Requests;
using BffPortalService.Application.DTOs.Responses;
using BffPortalService.Application.Services;
using BffPortalService.Domain.Exceptions;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace BffPortalService.Tests.Unit.Services;

public class TestsPortalServiceTests
{
    private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly Mock<ILogger<TestsPortalService>> _loggerMock;
    private readonly TestsPortalService _service;

    public TestsPortalServiceTests()
    {
        _httpClientFactoryMock = new Mock<IHttpClientFactory>();
        _cacheServiceMock = new Mock<ICacheService>();
        _loggerMock = new Mock<ILogger<TestsPortalService>>();
        _service = new TestsPortalService(
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
    public async Task InvalidateTestsCacheAsync_ShouldRemoveCacheByPrefix()
    {
        var userId = Guid.NewGuid();

        await _service.InvalidateTestsCacheAsync(userId, CancellationToken.None);

        _cacheServiceMock.Verify(
            x => x.RemoveByPrefixAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
