// OroKernel
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.
namespace OroKernel.Shared.Services;

public class IdentityClientService : IIdentityClientService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<IdentityClientService> _logger;
    private static readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    private const int MaxRetries = 3;
    private static readonly TimeSpan BaseDelay = TimeSpan.FromMilliseconds(200);

    public IdentityClientService(HttpClient httpClient, ILogger<IdentityClientService> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<RoleInfo?> GetRoleByIdAsync(Guid roleId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Get role by id request to identity server {RoleId}", roleId);
        using var response = await GetWithRetriesAsync($"api/getrolebyid/{roleId}", cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var stringResponse = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        var roleInfo = JsonSerializer.Deserialize<RoleInfo?>(stringResponse, _jsonOptions);
        return roleInfo;
    }

    public async Task<UserInfo?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Get user by id request to identity server {UserId}", userId);
        using var response = await GetWithRetriesAsync($"api/getuserbyid/{userId}", cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var stringResponse = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        var userInfo = JsonSerializer.Deserialize<UserInfo?>(stringResponse, _jsonOptions);
        return userInfo;
    }

    public async Task<IEnumerable<Guid>> GetUserRoleIdsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Get roles ids by user id request to identity server {UserId}", userId);
        using var response = await GetWithRetriesAsync($"api/getrolesidbyuserid/{userId}", cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var stringResponse = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        var roleIds = JsonSerializer.Deserialize<IEnumerable<Guid>>(stringResponse, _jsonOptions);
        return roleIds ?? System.Array.Empty<Guid>();
    }

    private async Task<HttpResponseMessage> GetWithRetriesAsync(string requestUri, CancellationToken cancellationToken)
    {
        for (var attempt = 1; ; attempt++)
        {
            try
            {
                var response = await _httpClient.GetAsync(requestUri, cancellationToken).ConfigureAwait(false);

                // Retry on server errors (5xx)
                if ((int)response.StatusCode >= 500 && attempt < MaxRetries)
                {
                    response.Dispose();
                    var delay = TimeSpan.FromMilliseconds(BaseDelay.TotalMilliseconds * Math.Pow(2, attempt - 1));
                    _logger.LogWarning("Transient server error fetching {RequestUri}. Attempt {Attempt} of {MaxRetries}. Retrying after {Delay}ms.", requestUri, attempt, MaxRetries, delay.TotalMilliseconds);
                    await Task.Delay(delay, cancellationToken).ConfigureAwait(false);
                    continue;
                }

                return response;
            }
            catch (HttpRequestException ex) when (attempt < MaxRetries)
            {
                var delay = TimeSpan.FromMilliseconds(BaseDelay.TotalMilliseconds * Math.Pow(2, attempt - 1));
                _logger.LogWarning(ex, "Transient error fetching {RequestUri}. Attempt {Attempt} of {MaxRetries}. Retrying after {Delay}ms.", requestUri, attempt, MaxRetries, delay.TotalMilliseconds);
                await Task.Delay(delay, cancellationToken).ConfigureAwait(false);
                continue;
            }
        }
    }
}