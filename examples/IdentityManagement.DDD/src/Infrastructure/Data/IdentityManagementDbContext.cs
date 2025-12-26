// IdentityManagementDbContext.cs - Infrastructure Data Context
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.

using IdentityManagement.DDD.Domain.Entities;
using IdentityManagement.DDD.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OroKernel.Shared.Data;

namespace IdentityManagement.DDD.Infrastructure.Data;

/// <summary>
/// EF Core DbContext for Identity Management bounded context
/// </summary>
public class IdentityManagementDbContext : AuditableDbContext
{
    public DbSet<IdentificationType> IdentificationTypes { get; set; }

    public IdentityManagementDbContext(DbContextOptions<IdentityManagementDbContext> options, IOptions<OroKernel.Shared.Options.UserInfo> userInfo)
        : base(options, userInfo)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure IdentificationType entity
        modelBuilder.Entity<IdentificationType>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasConversion(
                    id => id.Value,
                    value => new IdentificationTypeId(value))
                .HasMaxLength(36)
                .IsRequired();

            entity.Property(e => e.Name)
                .HasConversion(
                    v => v.Value,
                    v => IdentificationTypeName.Create(v))
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsRequired();

            entity.Property(e => e.CountryCode)
                .HasConversion(
                    v => v.Value,
                    v => CountryCode.Create(v))
                .HasMaxLength(3)
                .IsRequired();

            entity.Property(e => e.MaxLength)
                .IsRequired();

            entity.Property(e => e.ValidationPattern)
                .HasConversion(
                    v => v.Value,
                    v => ValidationPattern.CreatePattern(v))
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(e => e.IsActive)
                .IsRequired();

            // Indexes
            entity.HasIndex(e => e.Name).IsUnique();
            entity.HasIndex(e => e.CountryCode);
            entity.HasIndex(e => e.IsActive);
        });
    }
}