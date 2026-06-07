// OroKernel
// Copyright (C) 2026 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.
namespace OroKernel.Shared.Data;

/// <summary>
/// Auditable database context
/// </summary>
public abstract class AuditableDbContext : DbContext
{
    /// <summary>
    /// First index constant
    /// </summary>
    private const int First = 0;

    private readonly IUserInfoProvider _userInfoProvider;

    public AuditableDbContext(DbContextOptions options, IUserInfoProvider userInfoProvider) : base(options)
    {
        _userInfoProvider = userInfoProvider ?? throw new ArgumentNullException(nameof(userInfoProvider));
    }

    // Backwards-compatible constructor using IOptions<UserInfo>
    public AuditableDbContext(DbContextOptions options, IOptions<UserInfo> optionsUser)
        : this(options, new Services.DefaultUserInfoProvider(optionsUser))
    {
    }

    // Audit tables
    /// <summary>
    /// Audit entries table
    /// </summary>
    public DbSet<AuditEntry> AuditEntries { get; set; } = null!;
    /// <summary>
    /// Audit entry properties table
    /// </summary>
    public DbSet<AuditEntryProperty> AuditEntryProperties { get; set; } = null!;

    /// <summary>
    /// Saves all changes made in this context to the database asynchronously, with auditing.
    /// </summary>
    /// <param name="cancellationToken"> Cancellation token </param>
    /// <returns> Number of state entries written to the database </returns>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var auditEntries = OnBeforeSaveChanges();
        var result = await base.SaveChangesAsync(cancellationToken);
        await OnAfterSaveChanges(auditEntries);

        return result;
    }

    /// <summary>
    /// On before save changes, to track audit entries
    /// </summary>
    /// <returns>List of audit entries</returns>
    private List<AuditEntry> OnBeforeSaveChanges()
    {
        ChangeTracker.DetectChanges();
        var auditEntries = new List<AuditEntry>();

        // Obtain latest user info from provider at save-time (per-request)
        var currentUser = _userInfoProvider?.GetUserInfo();

        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is AuditEntry || entry.Entity is AuditEntryProperty)
                continue;

            if (entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                continue;

            var auditEntry = new AuditEntry
            {
                EntityName = entry.Metadata.GetTableName() ?? entry.Metadata.Name,
                Timestamp = DateTimeOffset.UtcNow,
                UserId = currentUser?.Id ?? Guid.Empty,
                UserName = currentUser?.UserName ?? string.Empty,
                State = entry.State
            };

            switch (entry.State)
            {
                case EntityState.Added:
                    auditEntry.Action = "Added";
                    break;
                case EntityState.Modified:
                    auditEntry.Action = "Modified";
                    foreach (var property in entry.Properties)
                    {
                        if (property.IsModified && !property.Metadata.IsForeignKey() && !property.Metadata.IsShadowProperty())
                        {
                            auditEntry.TemporaryProperties.Add(new PropertyChange
                            {
                                PropertyName = property.Metadata.Name,
                                OldValue = property.OriginalValue?.ToString(),
                                NewValue = property.CurrentValue?.ToString()
                            });
                        }
                    }
                    break;
                case EntityState.Deleted:
                    auditEntry.Action = "Deleted";
                    foreach (var property in entry.Properties)
                    {
                        auditEntry.TemporaryProperties.Add(new PropertyChange
                        {
                            PropertyName = property.Metadata.Name,
                            OldValue = property.OriginalValue?.ToString(),
                            NewValue = null
                        });
                    }
                    break;
            }
            auditEntries.Add(auditEntry);
        }
        return auditEntries;
    }

    /// <summary>
    /// On after save changes, to finalize audit entries
    /// </summary>
    /// <param name="auditEntries">List of audit entries</param>
    /// <returns>Task</returns>
    private async Task OnAfterSaveChanges(List<AuditEntry> auditEntries)
    {
        if (auditEntries == null || auditEntries.Count == 0) return;

        foreach (var auditEntry in auditEntries)
        {
            var entry = ChangeTracker.Entries().FirstOrDefault(e =>
                e.Metadata.GetTableName() == auditEntry.EntityName &&
                (e.State == auditEntry.State ||
                 (auditEntry.State == EntityState.Added && e.State == EntityState.Unchanged)));

            if (entry != null)
            {
                auditEntry.EntityId = entry.Property(entry.Metadata.FindPrimaryKey()!.Properties[First].Name).CurrentValue?.ToString() ?? "N/A";

                if (auditEntry.TemporaryProperties.Count != 0)
                {
                    auditEntry.ChangesJson = JsonSerializer.Serialize(auditEntry.TemporaryProperties);
                }

                foreach (var propChange in auditEntry.TemporaryProperties)
                {
                    auditEntry.Properties.Add(new AuditEntryProperty
                    {
                        PropertyName = propChange.PropertyName,
                        OldValue = propChange.OldValue,
                        NewValue = propChange.NewValue
                    });
                }
            }
            AuditEntries.Add(auditEntry);
        }

        await base.SaveChangesAsync();
    }

    /// <summary>
    /// On model creating database, and specific change model
    /// </summary>
    /// <param name="modelBuilder">Model builder application</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(assembly: Assembly.GetExecutingAssembly());
    }
}