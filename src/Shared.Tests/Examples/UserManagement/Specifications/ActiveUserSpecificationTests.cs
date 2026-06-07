using UserManagement;
using UserManagement.Specifications;

namespace Shared.Tests.Examples.UserManagement.Specifications;

public class ActiveUserSpecificationTests
{
    private readonly List<User> _users =
    [
        new() { UserName = "active1", IsActive = true },
        new() { UserName = "inactive1", IsActive = false },
        new() { UserName = "active2", IsActive = true },
    ];

    [Fact]
    public void IsSatisfiedBy_ActiveUser_ReturnsTrue()
    {
        var spec = new ActiveUserSpecification();
        Assert.True(spec.IsSatisfiedBy(_users[0]));
    }

    [Fact]
    public void IsSatisfiedBy_InactiveUser_ReturnsFalse()
    {
        var spec = new ActiveUserSpecification();
        Assert.False(spec.IsSatisfiedBy(_users[1]));
    }

    [Fact]
    public void ToExpression_FiltersActiveUsers()
    {
        var spec = new ActiveUserSpecification();
        var expr = spec.ToExpression().Compile();
        var result = _users.Where(expr).ToList();

        Assert.Equal(2, result.Count);
        Assert.All(result, u => Assert.True(u.IsActive));
    }
}
