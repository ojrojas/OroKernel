namespace OroKernel.Domain.Tests.Entities;

public class WithDomainEventBaseTests
{
    private class TestEntity : WithDomainEventBase { }

    private record TestDomainEvent : DomainEventBase { }

    [Fact]
    public void RaiseDomainEvent_AddsEventToCollection()
    {
        var entity = new TestEntity();
        var domainEvent = new TestDomainEvent();

        entity.RaiseDomainEvent(domainEvent);

        Assert.Single(entity.DomainEvents);
        Assert.Contains(domainEvent, entity.DomainEvents);
    }

    [Fact]
    public void ClearDomainEvents_RemovesAllEvents()
    {
        var entity = new TestEntity();
        entity.RaiseDomainEvent(new TestDomainEvent());
        entity.RaiseDomainEvent(new TestDomainEvent());

        entity.ClearDomainEvents();

        Assert.Empty(entity.DomainEvents);
    }

    [Fact]
    public void DomainEvents_DefaultsToEmpty()
    {
        var entity = new TestEntity();

        Assert.NotNull(entity.DomainEvents);
        Assert.Empty(entity.DomainEvents);
    }
}
