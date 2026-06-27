using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MSOptions = Microsoft.Extensions.Options;
using OroKernel.Infrastructure.Audit;
using OroKernel.Infrastructure.Options;

namespace OroKernel.Domain.Tests.Audit;

public class AuditableDbContextTests
{
    private class TestAuditableDbContext : AuditableDbContext
    {
        public DbSet<TestEntity> TestEntities { get; set; }

        public TestAuditableDbContext(DbContextOptions options, IOptions<UserInfo> userInfo)
            : base(options, userInfo) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }

    private class TestEntity : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
    }

    [Fact]
    public async Task SaveChangesAsync_AddsAuditEntry_ForAddedEntity()
    {
        var userInfo = new UserInfo { Id = Guid.NewGuid(), UserName = "TestUser" };
        var options = new DbContextOptionsBuilder<TestAuditableDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var userInfoOptions = MSOptions.Options.Create(userInfo);

        using var context = new TestAuditableDbContext(options, userInfoOptions);
        var entity = new TestEntity { Name = "Test" };

        context.Add(entity);
        await context.SaveChangesAsync();

        var auditEntry = await context.AuditEntries.FirstOrDefaultAsync();
        Assert.NotNull(auditEntry);
        Assert.Equal("Added", auditEntry.Action);
        Assert.Equal(userInfo.Id, auditEntry.UserId);
        Assert.Equal(userInfo.UserName, auditEntry.UserName);
    }

    [Fact]
    public async Task SaveChangesAsync_AddsAuditEntry_ForModifiedEntity()
    {
        var userInfo = new UserInfo { Id = Guid.NewGuid(), UserName = "TestUser" };
        var options = new DbContextOptionsBuilder<TestAuditableDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var userInfoOptions = MSOptions.Options.Create(userInfo);

        using var context = new TestAuditableDbContext(options, userInfoOptions);
        var entity = new TestEntity { Name = "Test" };
        context.Add(entity);
        await context.SaveChangesAsync();

        entity.Name = "Modified";
        context.Entry(entity).Property(e => e.Name).IsModified = true;
        await context.SaveChangesAsync();

        var auditEntries = await context.AuditEntries.ToListAsync();
        var modifyEntry = auditEntries.Last();
        Assert.Equal("Modified", modifyEntry.Action);
    }

    [Fact]
    public async Task SaveChangesAsync_AddsAuditEntry_ForDeletedEntity()
    {
        var userInfo = new UserInfo { Id = Guid.NewGuid(), UserName = "TestUser" };
        var options = new DbContextOptionsBuilder<TestAuditableDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var userInfoOptions = MSOptions.Options.Create(userInfo);

        using var context = new TestAuditableDbContext(options, userInfoOptions);
        var entity = new TestEntity { Name = "Test" };
        context.Add(entity);
        await context.SaveChangesAsync();

        context.Remove(entity);
        await context.SaveChangesAsync();

        var auditEntries = await context.AuditEntries.ToListAsync();
        var deleteEntry = auditEntries.Last();
        Assert.Equal("Deleted", deleteEntry.Action);
    }
}
