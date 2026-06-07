using Microsoft.Extensions.Logging.Abstractions;

namespace Shared.Tests.Services;

public class RetryDelegatingHandlerTests
{
    [Fact]
    public async Task SendAsync_OnSuccess_DoesNotRetry()
    {
        var innerHandler = new MockInnerHandler(HttpStatusCode.OK);
        var handler = new RetryDelegatingHandler(
            NullLogger<RetryDelegatingHandler>.Instance)
        {
            InnerHandler = innerHandler
        };

        using var invoker = new HttpMessageInvoker(handler);
        using var request = new HttpRequestMessage(HttpMethod.Get, "http://example.com");

        var response = await invoker.SendAsync(request, CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(1, innerHandler.CallCount);
    }

    [Fact]
    public async Task SendAsync_OnTransientServerError_RetriesUpToMaxTimes()
    {
        var innerHandler = new MockInnerHandler(
            HttpStatusCode.ServiceUnavailable,
            HttpStatusCode.ServiceUnavailable,
            HttpStatusCode.OK);
        var handler = new RetryDelegatingHandler(
            NullLogger<RetryDelegatingHandler>.Instance)
        {
            InnerHandler = innerHandler
        };

        using var invoker = new HttpMessageInvoker(handler);
        using var request = new HttpRequestMessage(HttpMethod.Get, "http://example.com");

        var response = await invoker.SendAsync(request, CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(3, innerHandler.CallCount);
    }

    [Fact]
    public async Task SendAsync_OnPersistentServerError_ReturnsErrorAfterMaxRetries()
    {
        var innerHandler = new MockInnerHandler(HttpStatusCode.ServiceUnavailable);
        var handler = new RetryDelegatingHandler(
            NullLogger<RetryDelegatingHandler>.Instance)
        {
            InnerHandler = innerHandler
        };

        using var invoker = new HttpMessageInvoker(handler);
        using var request = new HttpRequestMessage(HttpMethod.Get, "http://example.com");

        var response = await invoker.SendAsync(request, CancellationToken.None);

        Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);
        Assert.Equal(3, innerHandler.CallCount);
    }

    private sealed class MockInnerHandler : HttpMessageHandler
    {
        private readonly Queue<HttpStatusCode> _statusCodes;
        public int CallCount { get; private set; }

        public MockInnerHandler(params HttpStatusCode[] statusCodes)
        {
            _statusCodes = new Queue<HttpStatusCode>(statusCodes);
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            CallCount++;
            var statusCode = _statusCodes.Count > 0
                ? _statusCodes.Dequeue()
                : _statusCodes.TryPeek(out var last) ? last : HttpStatusCode.ServiceUnavailable;
            return Task.FromResult(new HttpResponseMessage(statusCode));
        }
    }
}
