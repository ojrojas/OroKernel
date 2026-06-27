using OroKernel.Infrastructure.Audit;

namespace OroKernel.Domain.Tests.Audit;

public class PropertyChangeTests
{
    [Fact]
    public void DefaultConstructor_InitializesPropertyNameToEmpty()
    {
        var change = new PropertyChange();

        Assert.Equal(string.Empty, change.PropertyName);
        Assert.Null(change.OldValue);
        Assert.Null(change.NewValue);
    }
}
