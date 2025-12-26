namespace Shared.Tests.Services;

public class ClaimsUserInfoServiceTests
{
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly ClaimsUserInfoService _service;

    public ClaimsUserInfoServiceTests()
    {
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        _service = new ClaimsUserInfoService(_httpContextAccessorMock.Object);
    }

    [Fact]
    public void PostConfigure_WithValidClaims_SetsUserInfoCorrectly()
    {
        // Arrange
        var claims = new List<Claim>
        {
            new("", "1"), // State
            new("Sub", Guid.NewGuid().ToString()),
            new("UserName", "testuser"),
            new("Email", "test@example.com")
        };
        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);
        var httpContext = new DefaultHttpContext { User = principal };
        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

        var options = new UserInfo { UserName = "" };

        // Act
        _service.PostConfigure("name", options);

        // Assert
        Assert.Equal(EntityBaseState.ACTIVE, options.State); // Assuming 1 is Active
        Assert.NotEqual(Guid.Empty, options.Id);
        Assert.Equal("testuser", options.UserName);
        Assert.Equal("test@example.com", options.Email);
    }

    [Fact]
    public void PostConfigure_WithInvalidStateClaim_SetsDefaultState()
    {
        // Arrange
        var claims = new List<Claim>
        {
            new("", "invalid"),
            new("Sub", Guid.NewGuid().ToString()),
            new("UserName", "testuser"),
            new("Email", "test@example.com")
        };
        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);
        var httpContext = new DefaultHttpContext { User = principal };
        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

        var options = new UserInfo { UserName = "" };

        // Act
        _service.PostConfigure("name", options);

        // Assert
        Assert.Equal(default(EntityBaseState), options.State);
    }

    [Fact]
    public void PostConfigure_WithNullHttpContext_DoesNotThrow()
    {
        // Arrange
        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns((HttpContext)null!);
        var options = new UserInfo { UserName = "" };

        // Act & Assert
        var exception = Record.Exception(() => _service.PostConfigure("name", options));
        Assert.Null(exception);
    }
}