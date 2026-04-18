
namespace Shared.Tests.Services;

public class IdentityClientServiceTests
{
    private readonly HttpClient _httpClient;
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private readonly Mock<ILogger<IdentityClientService>> _loggerMock;
    private readonly IdentityClientService _service;

    public IdentityClientServiceTests()
    {
        _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_httpMessageHandlerMock.Object) { BaseAddress = new Uri("http://localhost") };
        _loggerMock = new Mock<ILogger<IdentityClientService>>();
        _service = new IdentityClientService(_httpClient, _loggerMock.Object);
    }

    [Fact]
    public async Task GetRoleByIdAsync_ReturnsRoleInfo_WhenSuccessful()
    {
        // Arrange
        var roleId = Guid.NewGuid();
        var expectedRole = new RoleInfo { Id = roleId, Name = "TestRole" };
        var responseContent = JsonSerializer.Serialize(expectedRole);
        var httpResponseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(responseContent)
        };

        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponseMessage);

        // Act
        var result = await _service.GetRoleByIdAsync(roleId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedRole.Id, result.Id);
        Assert.Equal(expectedRole.Name, result.Name);
    }

    [Fact]
    public async Task GetRoleByIdAsync_ThrowsException_WhenResponseNotSuccessful()
    {
        // Arrange
        var roleId = Guid.NewGuid();
        var httpResponseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.NotFound
        };

        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponseMessage);

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() => _service.GetRoleByIdAsync(roleId));
    }

    [Fact]
    public async Task GetUserByIdAsync_ReturnsUserInfo_WhenSuccessful()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var expectedUser = new UserInfo { Id = userId, UserName = "TestUser", Email = "test@example.com" };
        var responseContent = JsonSerializer.Serialize(expectedUser);
        var httpResponseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(responseContent)
        };

        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponseMessage);

        // Act
        var result = await _service.GetUserByIdAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedUser.Id, result.Id);
        Assert.Equal(expectedUser.UserName, result.UserName);
    }

    [Fact]
    public async Task GetUserRoleIdsAsync_ReturnsRoleIds_WhenSuccessful()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var expectedRoleIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
        var responseContent = JsonSerializer.Serialize(expectedRoleIds);
        var httpResponseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(responseContent)
        };

        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponseMessage);

        // Act
        var result = await _service.GetUserRoleIdsAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedRoleIds, result);
    }

    [Fact]
    public async Task GetUserRoleIdsAsync_ThrowsException_WhenDeserializationFails()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var responseContent = "invalid json";
        var httpResponseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(responseContent)
        };

        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponseMessage);

        // Act & Assert
        await Assert.ThrowsAsync<System.Text.Json.JsonException>(() => _service.GetUserRoleIdsAsync(userId));
    }
}