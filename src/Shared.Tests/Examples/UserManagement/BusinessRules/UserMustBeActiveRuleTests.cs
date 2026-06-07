using UserManagement;
using UserManagement.BusinessRules;

namespace Shared.Tests.Examples.UserManagement.BusinessRules;

public class UserMustBeActiveRuleTests
{
    [Fact]
    public void IsSatisfied_WhenUserIsActive_ReturnsTrue()
    {
        var user = new User { IsActive = true };
        var rule = new UserMustBeActiveRule(user);

        Assert.True(rule.IsSatisfied());
        Assert.Null(rule.Error);
    }

    [Fact]
    public void IsSatisfied_WhenUserIsInactive_ReturnsFalseWithValidationError()
    {
        var user = new User { IsActive = false };
        var rule = new UserMustBeActiveRule(user);

        Assert.False(rule.IsSatisfied());
        Assert.NotNull(rule.Error);
        Assert.Equal("VALIDATION", rule.Error.Code);
    }
}
