

namespace Shared.Tests.Entities;

public class WithDomainEventBaseTests
{
    private class TestEntity : WithDomainEventBase { }

    [Fact]
    public void RaiseDomainEvent_AddsEventToCollection()
    {
        // Arrange
        var entity = new TestEntity();
        var domainEvent = new TestDomainEvent();

        // Act
        entity.RaiseDomainEvent(domainEvent);

        // Assert
        Assert.Single(entity.DomainEvents);
        Assert.Contains(domainEvent, entity.DomainEvents);
    }

    [Fact]
    public void ClearDomainEvents_RemovesAllEvents()
    {
        // Arrange
        var entity = new TestEntity();
        var domainEvent1 = new TestDomainEvent();
        var domainEvent2 = new TestDomainEvent();
        entity.RaiseDomainEvent(domainEvent1);
        entity.RaiseDomainEvent(domainEvent2);

        // Act
        entity.ClearDomainEvents();

        // Assert
        Assert.Empty(entity.DomainEvents);
    }

    [Fact]
    public void DomainEvents_IsReadOnly()
    {
        // Arrange
        var entity = new TestEntity();

        // Act & Assert
        Assert.IsAssignableFrom<IReadOnlyCollection<IDomainEvent>>(entity.DomainEvents);
    }

    private record TestDomainEvent : DomainEventBase;
}