using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MSOptions = Microsoft.Extensions.Options;
using OroKernel.Shared.Data;

namespace Shared.Tests.Data;

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
            // Configure test entities if needed
        }
    }

    private class TestEntity : OroKernel.Shared.Entities.BaseEntity
    {
        public string Name { get; set; } = string.Empty;
    }

    [Fact]
    public async Task SaveChangesAsync_AddsAuditEntry_ForAddedEntity()
    {
        // Arrange
        var userInfo = new UserInfo { Id = Guid.NewGuid(), UserName = "TestUser" };
        var options = new DbContextOptionsBuilder<TestAuditableDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var userInfoOptions = MSOptions.Options.Create(userInfo);

        using var context = new TestAuditableDbContext(options, userInfoOptions);
        var entity = new TestEntity { Name = "Test" };

        // Act
        context.Add(entity);
        await context.SaveChangesAsync();

        // Assert
        var auditEntry = await context.AuditEntries.FirstOrDefaultAsync();
        Assert.NotNull(auditEntry);
        Assert.Equal("Added", auditEntry.Action);
        Assert.Equal(userInfo.Id, auditEntry.UserId);
        Assert.Equal(userInfo.UserName, auditEntry.UserName);
    }

    [Fact]
    public async Task SaveChangesAsync_AddsAuditEntry_ForModifiedEntity()
    {
        // Arrange
        var userInfo = new UserInfo { Id = Guid.NewGuid(), UserName = "TestUser" };
        var options = new DbContextOptionsBuilder<TestAuditableDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var userInfoOptions = MSOptions.Options.Create(userInfo);

        using var context = new TestAuditableDbContext(options, userInfoOptions);
        var entity = new TestEntity { Name = "Test" };
        context.Add(entity);
        await context.SaveChangesAsync();

        // Act
        entity.Name = "Modified";
        context.Entry(entity).Property(e => e.Name).IsModified = true;
        await context.SaveChangesAsync();

        // Assert
        var auditEntries = await context.AuditEntries.ToListAsync();
        var modifyEntry = auditEntries.Last();
        Assert.Equal("Modified", modifyEntry.Action);
        // Note: ChangesJson may be null in in-memory database due to OriginalValue not being set
    }

    [Fact]
    public async Task SaveChangesAsync_AddsAuditEntry_ForDeletedEntity()
    {
        // Arrange
        var userInfo = new UserInfo { Id = Guid.NewGuid(), UserName = "TestUser" };
        var options = new DbContextOptionsBuilder<TestAuditableDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var userInfoOptions = MSOptions.Options.Create(userInfo);

        using var context = new TestAuditableDbContext(options, userInfoOptions);
        var entity = new TestEntity { Name = "Test" };
        context.Add(entity);
        await context.SaveChangesAsync();

        // Act
        context.Remove(entity);
        await context.SaveChangesAsync();

        // Assert
        var auditEntries = await context.AuditEntries.ToListAsync();
        var deleteEntry = auditEntries.Last();
        Assert.Equal("Deleted", deleteEntry.Action);
    }
}