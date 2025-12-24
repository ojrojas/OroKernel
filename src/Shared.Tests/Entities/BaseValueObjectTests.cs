
namespace Shared.Tests.Entities;

public class BaseValueObjectTests
{
    private record TestValueObject(string Value1, int Value2) : BaseValueObject
    {
        protected override IEnumerable<object?> GetEquatibilityComponents()
        {
            yield return Value1;
            yield return Value2;
        }
    }

    [Fact]
    public void Equals_SameValues_ReturnsTrue()
    {
        // Arrange
        var obj1 = new TestValueObject("test", 1);
        var obj2 = new TestValueObject("test", 1);

        // Act & Assert
        Assert.Equal(obj1, obj2);
    }

    [Fact]
    public void Equals_DifferentValues_ReturnsFalse()
    {
        // Arrange
        var obj1 = new TestValueObject("test", 1);
        var obj2 = new TestValueObject("different", 1);

        // Act & Assert
        Assert.NotEqual(obj1, obj2);
    }

    [Fact]
    public void GetHashCode_SameValues_ReturnsSameHash()
    {
        // Arrange
        var obj1 = new TestValueObject("test", 1);
        var obj2 = new TestValueObject("test", 1);

        // Act & Assert
        Assert.Equal(obj1.GetHashCode(), obj2.GetHashCode());
    }

    [Fact]
    public void GetHashCode_DifferentValues_ReturnsDifferentHash()
    {
        // Arrange
        var obj1 = new TestValueObject("test", 1);
        var obj2 = new TestValueObject("different", 1);

        // Act & Assert
        Assert.NotEqual(obj1.GetHashCode(), obj2.GetHashCode());
    }
}