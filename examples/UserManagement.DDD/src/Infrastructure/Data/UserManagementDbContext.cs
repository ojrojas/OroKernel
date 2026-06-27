// UserManagementDbContext.cs - Infrastructure Data Context
// Copyright (C) 2026 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OroKernel.Infrastructure.Audit;
using OroKernel.Infrastructure.Options;
using UserManagement.DDD.Domain.Entities;

namespace UserManagement.DDD.Infrastructure.Data;

/// <summary>
/// Database context for User Management bounded context
/// </summary>
public class UserManagementDbContext : AuditableDbContext
{
    public UserManagementDbContext(DbContextOptions<UserManagementDbContext> options, IOptions<UserInfo> userInfo)
        : base(options, userInfo)
    {
    }

    public DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure User entity
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.UserName).HasConversion(
                v => v.Value,
                v => Domain.ValueObjects.UserName.Create(v));
            entity.Property(u => u.FullName).HasConversion(
                v => $"{v.FirstName}|{v.LastName}",
                v => Domain.ValueObjects.FullName.Create(v.Substring(0, v.IndexOf('|')), v.Substring(v.IndexOf('|') + 1)));
            entity.Property(u => u.Email).HasConversion(
                v => v.Value,
                v => Domain.ValueObjects.Email.Create(v));
            entity.HasIndex(u => u.UserName).IsUnique();
            entity.HasIndex(u => u.Email).IsUnique();
        });
    }
}