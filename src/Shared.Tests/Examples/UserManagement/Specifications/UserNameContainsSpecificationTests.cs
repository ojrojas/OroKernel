using UserManagement;
using UserManagement.Specifications;

namespace Shared.Tests.Examples.UserManagement.Specifications;

public class UserNameContainsSpecificationTests
{
    private readonly List<User> _users =
    [
        new() { UserName = "john_doe" },
        new() { UserName = "jane_smith" },
        new() { UserName = "bob_wilson" },
    ];

    [Fact]
    public void IsSatisfiedBy_UserNameContainsSubstring_ReturnsTrue()
    {
        var spec = new UserNameContainsSpecification("john");
        Assert.True(spec.IsSatisfiedBy(_users[0]));
    }

    [Fact]
    public void IsSatisfiedBy_CaseInsensitiveMatch_ReturnsTrue()
    {
        var spec = new UserNameContainsSpecification("JOHN");
        Assert.True(spec.IsSatisfiedBy(_users[0]));
    }

    [Fact]
    public void IsSatisfiedBy_NoMatch_ReturnsFalse()
    {
        var spec = new UserNameContainsSpecification("xyz");
        Assert.False(spec.IsSatisfiedBy(_users[0]));
    }

    [Fact]
    public void ToExpression_FiltersUsersBySubstring()
    {
        var spec = new UserNameContainsSpecification("smith");
        var expr = spec.ToExpression().Compile();
        var result = _users.Where(expr).ToList();

        var match = Assert.Single(result);
        Assert.Equal("jane_smith", match.UserName);
    }
}
