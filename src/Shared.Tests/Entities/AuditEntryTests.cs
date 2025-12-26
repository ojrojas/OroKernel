using Microsoft.EntityFrameworkCore;
using OroKernel.Shared.Entities;

namespace Shared.Tests.Entities;

public class AuditEntryTests
{
    [Fact]
    public void Constructor_InitializesPropertiesWithDefaults()
    {
        // Arrange & Act
        var auditEntry = new AuditEntry();

        // Assert
        Assert.Equal(0, auditEntry.Id);
        Assert.Equal(string.Empty, auditEntry.EntityName);
        Assert.Equal(string.Empty, auditEntry.EntityId);
        Assert.Equal(string.Empty, auditEntry.Action);
        Assert.Equal(default(DateTimeOffset), auditEntry.Timestamp);
        Assert.Equal(Guid.Empty, auditEntry.UserId);
        Assert.Equal(string.Empty, auditEntry.UserName);
        Assert.Null(auditEntry.ChangesJson);
        Assert.NotNull(auditEntry.Properties);
        Assert.Empty(auditEntry.Properties);
        Assert.NotNull(auditEntry.TemporaryProperties);
        Assert.Empty(auditEntry.TemporaryProperties);
        Assert.Equal(default(EntityState), auditEntry.State);
    }

    [Fact]
    public void Properties_CanBeSetAndRetrieved()
    {
        // Arrange
        var auditEntry = new AuditEntry();
        var testId = 123;
        var entityName = "TestEntity";
        var entityId = "test-id";
        var action = "Modified";
        var timestamp = DateTimeOffset.UtcNow;
        var userId = Guid.NewGuid();
        var userName = "TestUser";
        var changesJson = "{\"property\":\"value\"}";
        var state = EntityState.Modified;

        // Act
        auditEntry.Id = testId;
        auditEntry.EntityName = entityName;
        auditEntry.EntityId = entityId;
        auditEntry.Action = action;
        auditEntry.Timestamp = timestamp;
        auditEntry.UserId = userId;
        auditEntry.UserName = userName;
        auditEntry.ChangesJson = changesJson;
        auditEntry.State = state;

        // Assert
        Assert.Equal(testId, auditEntry.Id);
        Assert.Equal(entityName, auditEntry.EntityName);
        Assert.Equal(entityId, auditEntry.EntityId);
        Assert.Equal(action, auditEntry.Action);
        Assert.Equal(timestamp, auditEntry.Timestamp);
        Assert.Equal(userId, auditEntry.UserId);
        Assert.Equal(userName, auditEntry.UserName);
        Assert.Equal(changesJson, auditEntry.ChangesJson);
        Assert.Equal(state, auditEntry.State);
    }

    [Fact]
    public void TemporaryProperties_CanAddAndRemoveItems()
    {
        // Arrange
        var auditEntry = new AuditEntry();
        var propertyChange = new PropertyChange
        {
            PropertyName = "TestProperty",
            OldValue = "old",
            NewValue = "new"
        };

        // Act
        auditEntry.TemporaryProperties.Add(propertyChange);

        // Assert
        Assert.Single(auditEntry.TemporaryProperties);
        Assert.Contains(propertyChange, auditEntry.TemporaryProperties);

        // Act
        auditEntry.TemporaryProperties.Remove(propertyChange);

        // Assert
        Assert.Empty(auditEntry.TemporaryProperties);
    }
}