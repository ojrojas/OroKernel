// IdentityManagementDbContext.cs
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OroKernel.Shared.Data;
using OroKernel.Shared.Options;

namespace IdentityManagement.Data;

public class IdentityManagementDbContext : AuditableDbContext
{
    public IdentityManagementDbContext(DbContextOptions<IdentityManagementDbContext> options, IOptions<UserInfo> userInfo)
        : base(options, userInfo)
    {
    }

    public DbSet<IdentificationType> IdentificationTypes { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure IdentificationType entity
        modelBuilder.Entity<IdentificationType>(entity =>
        {
            entity.HasKey(it => it.Id);
            entity.Property(it => it.Name).IsRequired().HasMaxLength(100);
            entity.Property(it => it.ValidationPattern).IsRequired().HasMaxLength(500);
            entity.Property(it => it.Description).HasMaxLength(500);
            entity.Property(it => it.CountryCode).HasMaxLength(3);
        });
    }
}