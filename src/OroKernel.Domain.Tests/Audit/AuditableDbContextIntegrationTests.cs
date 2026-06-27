using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MSOptions = Microsoft.Extensions.Options;
using OroKernel.Infrastructure.Audit;
using OroKernel.Infrastructure.Options;

namespace OroKernel.Domain.Tests.Audit;

public class AuditableDbContextIntegrationTests
{
    private class IntegrationTestDbContext : AuditableDbContext
    {
        public DbSet<IntegrationEntity> Items { get; set; }

        public IntegrationTestDbContext(DbContextOptions options, IOptions<UserInfo> userInfo)
            : base(options, userInfo) { }
    }

    private class IntegrationEntity : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
    }

    [Fact]
    public async Task FullLifecycle_ProducesAuditEntries_ForEveryOperation()
    {
        var userInfo = new UserInfo { Id = Guid.NewGuid(), UserName = "IntegrationUser" };
        var options = new DbContextOptionsBuilder<IntegrationTestDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var userInfoOptions = MSOptions.Options.Create(userInfo);

        using var context = new IntegrationTestDbContext(options, userInfoOptions);

        var entity = new IntegrationEntity { Name = "First" };
        await context.Items.AddAsync(entity);
        await context.SaveChangesAsync();

        entity.Name = "Updated";
        context.Entry(entity).Property(e => e.Name).IsModified = true;
        await context.SaveChangesAsync();

        context.Items.Remove(entity);
        await context.SaveChangesAsync();

        var audits = await context.AuditEntries.OrderBy(a => a.Id).ToListAsync();
        Assert.Equal(3, audits.Count);
        Assert.Equal("Added", audits[0].Action);
        Assert.Equal("Modified", audits[1].Action);
        Assert.Equal("Deleted", audits[2].Action);
        Assert.All(audits, a => Assert.Equal(userInfo.UserName, a.UserName));
    }
}
