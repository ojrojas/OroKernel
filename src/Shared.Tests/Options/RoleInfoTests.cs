using OroKernel.Shared.Options;

namespace Shared.Tests.Options;

public class RoleInfoTests
{
    [Fact]
    public void Constructor_InitializesPropertiesWithDefaults()
    {
        // Arrange & Act
        var roleInfo = new RoleInfo();

        // Assert
        Assert.Equal(Guid.Empty, roleInfo.Id);
        Assert.Equal(string.Empty, roleInfo.Name);
    }

    [Fact]
    public void Properties_CanBeSetAndRetrieved()
    {
        // Arrange
        var roleInfo = new RoleInfo();
        var testId = Guid.NewGuid();
        var testName = "Administrator";

        // Act
        roleInfo.Id = testId;
        roleInfo.Name = testName;

        // Assert
        Assert.Equal(testId, roleInfo.Id);
        Assert.Equal(testName, roleInfo.Name);
    }

    [Fact]
    public void Name_CanBeSetToNullOrEmpty()
    {
        // Arrange
        var roleInfo = new RoleInfo();

        // Act
        roleInfo.Name = null!;

        // Assert
        Assert.Null(roleInfo.Name);

        // Act
        roleInfo.Name = string.Empty;

        // Assert
        Assert.Equal(string.Empty, roleInfo.Name);
    }
}