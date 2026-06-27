using Microsoft.Extensions.Options;
using OroKernel.Infrastructure.Options;
using OroKernel.Infrastructure.Services;

namespace OroKernel.Domain.Tests.Services;

public class DefaultUserInfoProviderTests
{
    [Fact]
    public void GetUserInfo_ReturnsConfiguredValue()
    {
        var expected = new UserInfo { Id = Guid.NewGuid(), UserName = "bob" };
        var sut = new DefaultUserInfoProvider(Options.Create(expected));

        var actual = sut.GetUserInfo();

        Assert.NotNull(actual);
        Assert.Equal(expected.Id, actual!.Id);
        Assert.Equal(expected.UserName, actual.UserName);
    }
}
