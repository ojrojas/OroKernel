using Microsoft.EntityFrameworkCore;
using UserManagement;
using UserManagement.BusinessRules;
using UserManagement.Data;
using OptionsFactory = Microsoft.Extensions.Options.Options;

namespace Shared.Tests.Examples.UserManagement.BusinessRules;

public class UserEmailMustBeUniqueRuleTests
{
    [Fact]
    public void IsSatisfied_WhenEmailIsUnique_ReturnsTrue()
    {
        using var db = CreateDbContext();

        var rule = new UserEmailMustBeUniqueRule("unique@example.com", db);

        Assert.True(rule.IsSatisfied());
        Assert.Null(rule.Error);
    }

    [Fact]
    public void IsSatisfied_WhenEmailAlreadyExists_ReturnsFalseWithConflictError()
    {
        using var db = CreateDbContext();
        db.Users.Add(new User { UserName = "existing", Email = "taken@example.com", FirstName = "Existing", LastName = "User" });
        db.SaveChanges();

        var rule = new UserEmailMustBeUniqueRule("taken@example.com", db);

        Assert.False(rule.IsSatisfied());
        Assert.NotNull(rule.Error);
        Assert.Equal("CONFLICT", rule.Error.Code);
    }

    private static UserManagementDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<UserManagementDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var userInfo = OptionsFactory.Create(new UserInfo { Id = Guid.NewGuid(), UserName = "test" });
        return new UserManagementDbContext(options, userInfo);
    }
}
