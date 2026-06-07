using UserManagement;
using UserManagement.Specifications;

namespace Shared.Tests.Examples.UserManagement.Specifications;

public class UserByEmailSpecificationTests
{
    private readonly List<User> _users =
    [
        new() { UserName = "john", Email = "john@example.com" },
        new() { UserName = "jane", Email = "jane@example.com" },
    ];

    [Fact]
    public void IsSatisfiedBy_ExactEmailMatch_ReturnsTrue()
    {
        var spec = new UserByEmailSpecification("john@example.com");
        Assert.True(spec.IsSatisfiedBy(_users[0]));
    }

    [Fact]
    public void IsSatisfiedBy_CaseInsensitiveMatch_ReturnsTrue()
    {
        var spec = new UserByEmailSpecification("JOHN@EXAMPLE.COM");
        Assert.True(spec.IsSatisfiedBy(_users[0]));
    }

    [Fact]
    public void IsSatisfiedBy_NoMatch_ReturnsFalse()
    {
        var spec = new UserByEmailSpecification("notfound@example.com");
        Assert.False(spec.IsSatisfiedBy(_users[0]));
    }

    [Fact]
    public void ToExpression_FiltersByEmail()
    {
        var spec = new UserByEmailSpecification("jane@example.com");
        var expr = spec.ToExpression().Compile();
        var result = _users.Where(expr).ToList();

        var match = Assert.Single(result);
        Assert.Equal("jane", match.UserName);
    }
}
