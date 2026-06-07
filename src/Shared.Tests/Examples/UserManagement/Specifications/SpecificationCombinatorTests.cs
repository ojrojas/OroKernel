using UserManagement;
using UserManagement.Specifications;

namespace Shared.Tests.Examples.UserManagement.Specifications;

public class SpecificationCombinatorTests
{
    private readonly List<User> _users =
    [
        new() { UserName = "alice", IsActive = true, Email = "alice@example.com" },
        new() { UserName = "bob", IsActive = false, Email = "bob@example.com" },
        new() { UserName = "charlie", IsActive = true, Email = "charlie@example.com" },
    ];

    [Fact]
    public void And_ReturnsIntersection()
    {
        var active = new ActiveUserSpecification();
        var emailSpec = new UserByEmailSpecification("alice@example.com");
        var combined = active.And(emailSpec);

        var result = _users.Where(u => combined.IsSatisfiedBy(u)).ToList();

        var match = Assert.Single(result);
        Assert.Equal("alice", match.UserName);
    }

    [Fact]
    public void Or_ReturnsUnion()
    {
        var active = new ActiveUserSpecification();
        var inactive = active.Not();
        var combined = active.Or(inactive);

        var result = _users.Where(u => combined.IsSatisfiedBy(u)).ToList();

        Assert.Equal(3, result.Count);
    }

    [Fact]
    public void Not_InvertsCondition()
    {
        var active = new ActiveUserSpecification();
        var inactive = active.Not();

        var result = _users.Where(u => inactive.IsSatisfiedBy(u)).ToList();

        var match = Assert.Single(result);
        Assert.Equal("bob", match.UserName);
    }

    [Fact]
    public void And_EmptyIntersection_ReturnsNoResults()
    {
        var active = new ActiveUserSpecification();
        var emailSpec = new UserByEmailSpecification("bob@example.com");
        var combined = active.And(emailSpec);

        var result = _users.Where(u => combined.IsSatisfiedBy(u)).ToList();

        Assert.Empty(result);
    }

    [Fact]
    public void ComplexExpression_ActiveAndEmailOrName_ReturnsCorrectResults()
    {
        var active = new ActiveUserSpecification();
        var emailSpec = new UserByEmailSpecification("alice@example.com");
        var nameSpec = new UserNameContainsSpecification("charlie");

        var combined = active.And(emailSpec.Or(nameSpec));

        var result = _users.Where(u => combined.IsSatisfiedBy(u)).ToList();

        Assert.Equal(2, result.Count);
        Assert.Contains(result, u => u.UserName == "alice");
        Assert.Contains(result, u => u.UserName == "charlie");
    }
}
