// UserManagementDbContext.cs
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OroKernel.Shared.Data;
using OroKernel.Shared.Options;

namespace UserManagement.Data;

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
            entity.Property(u => u.UserName).IsRequired().HasMaxLength(50);
            entity.Property(u => u.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(u => u.LastName).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(255);
            entity.HasIndex(u => u.UserName).IsUnique();
            entity.HasIndex(u => u.Email).IsUnique();
        });
    }
}