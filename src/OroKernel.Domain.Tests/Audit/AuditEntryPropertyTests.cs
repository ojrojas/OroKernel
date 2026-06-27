using OroKernel.Infrastructure.Audit;

namespace OroKernel.Domain.Tests.Audit;

public class AuditEntryPropertyTests
{
    [Fact]
    public void DefaultConstructor_InitializesRequiredStrings()
    {
        var property = new AuditEntryProperty();

        Assert.Equal(0, property.Id);
        Assert.Equal(0, property.AuditEntryId);
        Assert.Equal(string.Empty, property.PropertyName);
        Assert.Null(property.OldValue);
        Assert.Null(property.NewValue);
    }
}
