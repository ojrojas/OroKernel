using OroKernel.Shared.Entities;

namespace Shared.Tests.Entities;

public class PropertyChangeTests
{
    [Fact]
    public void Constructor_InitializesPropertiesWithDefaults()
    {
        // Arrange & Act
        var propertyChange = new PropertyChange();

        // Assert
        Assert.Equal(string.Empty, propertyChange.PropertyName);
        Assert.Null(propertyChange.OldValue);
        Assert.Null(propertyChange.NewValue);
    }

    [Fact]
    public void Properties_CanBeSetAndRetrieved()
    {
        // Arrange
        var propertyChange = new PropertyChange();
        var propertyName = "TestProperty";
        var oldValue = "old value";
        var newValue = "new value";

        // Act
        propertyChange.PropertyName = propertyName;
        propertyChange.OldValue = oldValue;
        propertyChange.NewValue = newValue;

        // Assert
        Assert.Equal(propertyName, propertyChange.PropertyName);
        Assert.Equal(oldValue, propertyChange.OldValue);
        Assert.Equal(newValue, propertyChange.NewValue);
    }

    [Fact]
    public void Properties_CanHandleNullValues()
    {
        // Arrange
        var propertyChange = new PropertyChange();

        // Act
        propertyChange.OldValue = null;
        propertyChange.NewValue = null;

        // Assert
        Assert.Null(propertyChange.OldValue);
        Assert.Null(propertyChange.NewValue);
    }
}