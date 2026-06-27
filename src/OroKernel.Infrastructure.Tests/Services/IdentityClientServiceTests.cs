using System.Net;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Text.Json;
using OroKernel.Infrastructure.Options;
using OroKernel.Infrastructure.Services;

namespace OroKernel.Domain.Tests.Services;

public class IdentityClientServiceTests
{
    [Fact]
    public async Task GetUserByIdAsync_ReturnsDeserializedUser_OnSuccess()
    {
        var expected = new UserInfo { Id = Guid.NewGuid(), UserName = "alice" };
        var json = JsonSerializer.Serialize(expected);
        var handler = BuildHandler(HttpStatusCode.OK, json);

        var httpClient = new HttpClient(handler) { BaseAddress = new Uri("https://identity.test/") };
        var logger = new Mock<ILogger<IdentityClientService>>().Object;
        var sut = new IdentityClientService(httpClient, logger);

        var actual = await sut.GetUserByIdAsync(expected.Id);

        Assert.NotNull(actual);
        Assert.Equal(expected.Id, actual!.Id);
        Assert.Equal(expected.UserName, actual.UserName);
    }

    [Fact]
    public async Task GetRoleByIdAsync_ReturnsDeserializedRole_OnSuccess()
    {
        var role = new RoleInfo { Id = Guid.NewGuid(), Name = "Admin" };
        var json = JsonSerializer.Serialize(role);
        var handler = BuildHandler(HttpStatusCode.OK, json);

        var httpClient = new HttpClient(handler) { BaseAddress = new Uri("https://identity.test/") };
        var logger = new Mock<ILogger<IdentityClientService>>().Object;
        var sut = new IdentityClientService(httpClient, logger);

        var actual = await sut.GetRoleByIdAsync(role.Id);

        Assert.NotNull(actual);
        Assert.Equal(role.Id, actual!.Id);
        Assert.Equal(role.Name, actual.Name);
    }

    [Fact]
    public async Task GetUserRoleIdsAsync_ReturnsEmptyArray_WhenBodyIsEmpty()
    {
        var handler = BuildHandler(HttpStatusCode.OK, "null");
        var httpClient = new HttpClient(handler) { BaseAddress = new Uri("https://identity.test/") };
        var logger = new Mock<ILogger<IdentityClientService>>().Object;
        var sut = new IdentityClientService(httpClient, logger);

        var actual = await sut.GetUserRoleIdsAsync(Guid.NewGuid());

        Assert.NotNull(actual);
        Assert.Empty(actual);
    }

    private static HttpMessageHandler BuildHandler(HttpStatusCode status, string body)
    {
        var mock = new Mock<HttpMessageHandler>();
        mock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(status) { Content = new StringContent(body) });
        return mock.Object;
    }
}
