// OroKernel
// Copyright (C) 2026 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.
namespace OroKernel.Shared.Services;

public class RetryDelegatingHandler : DelegatingHandler
{
    private const int MaxRetries = 3;
    private static readonly TimeSpan BaseDelay = TimeSpan.FromMilliseconds(200);
    private readonly ILogger<RetryDelegatingHandler> _logger;

    public RetryDelegatingHandler(ILogger<RetryDelegatingHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        for (var attempt = 1; ; attempt++)
        {
            try
            {
                var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

                if ((int)response.StatusCode >= 500 && attempt < MaxRetries)
                {
                    _logger.LogWarning("Transient server error {StatusCode} on {Request}. Attempt {Attempt}. Retrying...", response.StatusCode, request.RequestUri, attempt);
                    response.Dispose();
                    var delay = TimeSpan.FromMilliseconds(BaseDelay.TotalMilliseconds * Math.Pow(2, attempt - 1));
                    await Task.Delay(delay, cancellationToken).ConfigureAwait(false);
                    continue;
                }

                return response;
            }
            catch (HttpRequestException ex) when (attempt < MaxRetries)
            {
                _logger.LogWarning(ex, "Request error on {Request}. Attempt {Attempt}. Retrying...", request.RequestUri, attempt);
                var delay = TimeSpan.FromMilliseconds(BaseDelay.TotalMilliseconds * Math.Pow(2, attempt - 1));
                await Task.Delay(delay, cancellationToken).ConfigureAwait(false);
                continue;
            }
        }
    }
}