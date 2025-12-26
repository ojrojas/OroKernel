using OroKernel.Shared.Entities;
using OroKernel.Shared.Options;

namespace Shared.Tests.Options;

public class UserInfoTests
{
    [Fact]
    public void Constructor_InitializesPropertiesWithDefaults()
    {
        // Arrange & Act
        var userInfo = new UserInfo { UserName = "test" };

        // Assert
        Assert.Equal(Guid.Empty, userInfo.Id);
        Assert.Equal("test", userInfo.UserName);
        Assert.Equal(default(EntityBaseState), userInfo.State);
        Assert.Equal(string.Empty, userInfo.Email);
    }

    [Fact]
    public void Properties_CanBeSetAndRetrieved()
    {
        // Arrange
        var userInfo = new UserInfo { UserName = "test" };
        var testId = Guid.NewGuid();
        var testUserName = "TestUser";
        var testState = EntityBaseState.ACTIVE;
        var testEmail = "test@example.com";

        // Act
        userInfo.Id = testId;
        userInfo.UserName = testUserName;
        userInfo.State = testState;
        userInfo.Email = testEmail;

        // Assert
        Assert.Equal(testId, userInfo.Id);
        Assert.Equal(testUserName, userInfo.UserName);
        Assert.Equal(testState, userInfo.State);
        Assert.Equal(testEmail, userInfo.Email);
    }

    [Fact]
    public void RequiredProperty_UserName_MustBeSet()
    {
        // Arrange & Act & Assert
        // This would normally fail at compile time, but we can test the behavior
        var userInfo = new UserInfo { UserName = "required" };
        Assert.Equal("required", userInfo.UserName);
    }

    [Fact]
    public void Email_CanBeSetToNullOrEmpty()
    {
        // Arrange
        var userInfo = new UserInfo { UserName = "test" };

        // Act
        userInfo.Email = null!;

        // Assert
        Assert.Null(userInfo.Email);

        // Act
        userInfo.Email = string.Empty;

        // Assert
        Assert.Equal(string.Empty, userInfo.Email);
    }
}