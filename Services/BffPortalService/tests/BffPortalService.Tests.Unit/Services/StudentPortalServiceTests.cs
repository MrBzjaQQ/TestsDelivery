using BffPortalService.Application.Contracts;
using BffPortalService.Application.DTOs.Responses;
using BffPortalService.Application.Services;
using BffPortalService.Domain.Exceptions;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using Xunit;

namespace BffPortalService.Tests.Unit.Services;

public class StudentPortalServiceTests
{
    private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly Mock<ILogger<StudentPortalService>> _loggerMock;
    private readonly StudentPortalService _service;

    public StudentPortalServiceTests()
    {
        _httpClientFactoryMock = new Mock<IHttpClientFactory>();
        _cacheServiceMock = new Mock<ICacheService>();
        _loggerMock = new Mock<ILogger<StudentPortalService>>();
        _service = new StudentPortalService(
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
    public async Task InvalidateStudentCacheAsync_ShouldRemoveCache()
    {
        var studentId = Guid.NewGuid();

        await _service.InvalidateStudentCacheAsync(studentId, CancellationToken.None);

        _cacheServiceMock.Verify(
            x => x.RemoveAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}

internal class MockHttpMessageHandler : HttpMessageHandler
{
    private readonly Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> _sendAsync;

    public MockHttpMessageHandler(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsync)
    {
        _sendAsync = sendAsync;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return _sendAsync(request, cancellationToken);
    }
}
