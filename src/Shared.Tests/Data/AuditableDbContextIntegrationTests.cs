using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OroKernel.Shared.Data;

namespace Shared.Tests.Data;

public class AuditableDbContextIntegrationTests : IDisposable
{
    private readonly TestDbContext _context;
    private readonly Mock<IUserInfoProvider> _userInfoProviderMock;

    public AuditableDbContextIntegrationTests()
    {
        _userInfoProviderMock = new Mock<IUserInfoProvider>();
        _userInfoProviderMock
            .Setup(x => x.GetUserInfo())
            .Returns(new UserInfo { Id = Guid.NewGuid(), UserName = "test-user" });

        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new TestDbContext(options, _userInfoProviderMock.Object);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Fact]
    public async Task SaveChangesAsync_AddedEntity_CreatesAuditEntry()
    {
        _context.TestEntities.Add(new TestEntity { Name = "new-entity" });
        await _context.SaveChangesAsync();

        var auditEntries = await _context.AuditEntries.ToListAsync();
        Assert.Single(auditEntries);
        Assert.Equal("Added", auditEntries[0].Action);
        Assert.Equal("test-user", auditEntries[0].UserName);
    }

    [Fact]
    public async Task SaveChangesAsync_ModifiedEntity_CreatesAuditEntry()
    {
        var entity = new TestEntity { Name = "original" };
        _context.TestEntities.Add(entity);
        await _context.SaveChangesAsync();

        entity.Name = "modified";
        await _context.SaveChangesAsync();

        var auditEntries = await _context.AuditEntries.ToListAsync();
        Assert.Equal(2, auditEntries.Count);
        Assert.Contains(auditEntries, a => a.Action == "Added");
        Assert.Contains(auditEntries, a => a.Action == "Modified");
    }

    [Fact]
    public async Task SaveChangesAsync_DeletedEntity_CreatesAuditEntry()
    {
        var entity = new TestEntity { Name = "to-delete" };
        _context.TestEntities.Add(entity);
        await _context.SaveChangesAsync();

        _context.TestEntities.Remove(entity);
        await _context.SaveChangesAsync();

        var auditEntries = await _context.AuditEntries.ToListAsync();
        Assert.Equal(2, auditEntries.Count);
        Assert.Contains(auditEntries, a => a.Action == "Added");
        Assert.Contains(auditEntries, a => a.Action == "Deleted");
    }

    [Fact]
    public async Task SaveChangesAsync_MultipleChanges_CreatesAuditEntriesForEach()
    {
        _context.TestEntities.Add(new TestEntity { Name = "first" });
        _context.TestEntities.Add(new TestEntity { Name = "second" });
        _context.TestEntities.Add(new TestEntity { Name = "third" });
        await _context.SaveChangesAsync();

        var auditEntries = await _context.AuditEntries.ToListAsync();
        Assert.Equal(3, auditEntries.Count);
        Assert.All(auditEntries, a => Assert.Equal("Added", a.Action));
    }

    [Fact]
    public async Task SaveChangesAsync_UnauthenticatedUser_CreatesAuditEntryWithEmptyUser()
    {
        _userInfoProviderMock
            .Setup(x => x.GetUserInfo())
            .Returns((UserInfo?)null);

        _context.TestEntities.Add(new TestEntity { Name = "anonymous" });
        await _context.SaveChangesAsync();

        var auditEntries = await _context.AuditEntries.ToListAsync();
        var entry = Assert.Single(auditEntries);
        Assert.Equal(Guid.Empty, entry.UserId);
        Assert.Empty(entry.UserName);
    }

    [Fact]
    public async Task SaveChangesAsync_AuditEntryHasEntityId()
    {
        var entity = new TestEntity { Name = "with-id" };
        _context.TestEntities.Add(entity);
        await _context.SaveChangesAsync();

        var auditEntries = await _context.AuditEntries.ToListAsync();
        var entry = Assert.Single(auditEntries);
        Assert.NotNull(entry.EntityId);
        Assert.NotEmpty(entry.EntityId);
        Assert.NotEqual("N/A", entry.EntityId);
    }

    private sealed class TestDbContext : AuditableDbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options, IUserInfoProvider userInfoProvider)
            : base(options, userInfoProvider) { }

        public DbSet<TestEntity> TestEntities { get; set; } = null!;
    }

    private sealed class TestEntity : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
    }
}
