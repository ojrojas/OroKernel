using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using OroKernel.Infrastructure.Options;
using OroKernel.Infrastructure.Services;
using System.Security.Claims;

namespace OroKernel.Domain.Tests.Services;

public class ClaimsUserInfoServiceTests
{
    [Fact]
    public void PostConfigure_PopulatesFromClaims_WhenUserIsAuthenticated()
    {
        var userId = Guid.NewGuid();
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Name, "alice"),
            new Claim(ClaimTypes.Email, "alice@example.com")
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var principal = new ClaimsPrincipal(identity);

        var httpContext = new DefaultHttpContext { User = principal };
        var accessor = new HttpContextAccessor { HttpContext = httpContext };

        var sut = new ClaimsUserInfoService(accessor);
        var options = new UserInfo { UserName = string.Empty };

        sut.PostConfigure(null, options);

        Assert.Equal(userId, options.Id);
        Assert.Equal("alice", options.UserName);
        Assert.Equal("alice@example.com", options.Email);
    }

    [Fact]
    public void PostConfiguration_LeavesDefaults_WhenHttpContextIsNull()
    {
        var accessor = new HttpContextAccessor { HttpContext = null };
        var sut = new ClaimsUserInfoService(accessor);
        var options = new UserInfo { Id = Guid.NewGuid(), UserName = "Original" };

        sut.PostConfigure(null, options);

        Assert.Equal("Original", options.UserName);
    }
}
