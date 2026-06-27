using OroKernel.Infrastructure.Audit;

namespace OroKernel.Domain.Tests.Audit;

public class AuditEntryTests
{
    [Fact]
    public void DefaultConstructor_InitializesCollectionsAndDefaults()
    {
        var entry = new AuditEntry();

        Assert.Equal(0, entry.Id);
        Assert.Equal(string.Empty, entry.EntityName);
        Assert.Equal(string.Empty, entry.EntityId);
        Assert.Equal(string.Empty, entry.Action);
        Assert.Equal(Guid.Empty, entry.UserId);
        Assert.Equal(string.Empty, entry.UserName);
        Assert.Null(entry.ChangesJson);
        Assert.NotNull(entry.Properties);
        Assert.Empty(entry.Properties);
        Assert.NotNull(entry.TemporaryProperties);
        Assert.Empty(entry.TemporaryProperties);
    }
}
