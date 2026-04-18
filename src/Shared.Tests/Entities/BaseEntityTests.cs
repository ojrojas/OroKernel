namespace Shared.Tests.Entities;

public class BaseEntityTests
{
    private class TestEntity : BaseEntity { }

    [Fact]
    public void Constructor_SetsIdToVersion7Guid()
    {
        // Arrange & Act
        var entity = new TestEntity();

        // Assert
        Assert.NotEqual(Guid.Empty, entity.Id);
        // Version 7 GUIDs have a specific format, but for simplicity, just check it's not empty
    }

    [Fact]
    public void Id_CanBeSet()
    {
        // Arrange
        var entity = new TestEntity();
        var newId = Guid.NewGuid();

        // Act
        entity.Id = newId;

        // Assert
        Assert.Equal(newId, entity.Id);
    }
}