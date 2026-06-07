using UserManagement;
using UserManagement.BusinessRules;

namespace Shared.Tests.Examples.UserManagement.BusinessRules;

public class MinimumUserNameLengthRuleTests
{
    [Fact]
    public void IsSatisfied_WhenUserNameIsLongEnough_ReturnsTrue()
    {
        var rule = new MinimumUserNameLengthRule("john_doe");

        Assert.True(rule.IsSatisfied());
        Assert.Null(rule.Error);
    }

    [Fact]
    public void IsSatisfied_WhenUserNameIsTooShort_ReturnsFalseWithValidationError()
    {
        var rule = new MinimumUserNameLengthRule("ab");

        Assert.False(rule.IsSatisfied());
        Assert.NotNull(rule.Error);
        Assert.Equal("VALIDATION", rule.Error.Code);
    }

    [Fact]
    public void IsSatisfied_WhenUserNameIsExactlyThreeCharacters_ReturnsTrue()
    {
        var rule = new MinimumUserNameLengthRule("abc");

        Assert.True(rule.IsSatisfied());
        Assert.Null(rule.Error);
    }
}
