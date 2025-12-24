
namespace Shared.Tests.Events;

public class DomainEventBaseTests
{
    private record TestDomainEvent : DomainEventBase;

    [Fact]
    public void Constructor_SetsOccurredOnToUtcNow()
    {
        // Arrange
        var before = DateTime.UtcNow;

        // Act
        var domainEvent = new TestDomainEvent();

        // Assert
        Assert.True(domainEvent.OcurredOn >= before);
        Assert.True(domainEvent.OcurredOn <= DateTime.UtcNow);
    }

    [Fact]
    public void CorrelationId_ReturnsNewGuid()
    {
        // Arrange
        var domainEvent = new TestDomainEvent();

        // Act
        var correlationId = domainEvent.CorrelationId();

        // Assert
        Assert.NotEqual(Guid.Empty, correlationId);
    }
}