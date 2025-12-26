using OroKernel.Shared.Entities;

namespace Shared.Tests.Entities;

public class AuditEntryPropertyTests
{
    [Fact]
    public void Constructor_InitializesPropertiesWithDefaults()
    {
        // Arrange & Act
        var auditEntryProperty = new AuditEntryProperty();

        // Assert
        Assert.Equal(0, auditEntryProperty.Id);
        Assert.Equal(0, auditEntryProperty.AuditEntryId);
        Assert.Equal(string.Empty, auditEntryProperty.PropertyName);
        Assert.Null(auditEntryProperty.OldValue);
        Assert.Null(auditEntryProperty.NewValue);
        // AuditEntry is a navigation property, typically set by EF, so it may be null
    }

    [Fact]
    public void Properties_CanBeSetAndRetrieved()
    {
        // Arrange
        var auditEntryProperty = new AuditEntryProperty();
        var testId = 456;
        var auditEntryId = 123;
        var propertyName = "TestProperty";
        var oldValue = "old value";
        var newValue = "new value";
        var auditEntry = new AuditEntry();

        // Act
        auditEntryProperty.Id = testId;
        auditEntryProperty.AuditEntryId = auditEntryId;
        auditEntryProperty.PropertyName = propertyName;
        auditEntryProperty.OldValue = oldValue;
        auditEntryProperty.NewValue = newValue;
        auditEntryProperty.AuditEntry = auditEntry;

        // Assert
        Assert.Equal(testId, auditEntryProperty.Id);
        Assert.Equal(auditEntryId, auditEntryProperty.AuditEntryId);
        Assert.Equal(propertyName, auditEntryProperty.PropertyName);
        Assert.Equal(oldValue, auditEntryProperty.OldValue);
        Assert.Equal(newValue, auditEntryProperty.NewValue);
        Assert.Equal(auditEntry, auditEntryProperty.AuditEntry);
    }
}