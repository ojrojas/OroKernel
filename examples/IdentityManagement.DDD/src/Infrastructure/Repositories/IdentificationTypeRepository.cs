// IdentificationTypeRepository.cs - Infrastructure Repository Implementation
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.

using IdentityManagement.DDD.Domain.Entities;
using IdentityManagement.DDD.Domain.Repositories;
using IdentityManagement.DDD.Domain.ValueObjects;
using IdentityManagement.DDD.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace IdentityManagement.DDD.Infrastructure.Repositories;

/// <summary>
/// EF Core implementation of IIdentificationTypeRepository
/// </summary>
public class IdentificationTypeRepository : IIdentificationTypeRepository
{
    private readonly IdentityManagementDbContext _context;

    public IdentificationTypeRepository(IdentityManagementDbContext context)
    {
        _context = context;
    }

    public async Task<IdentificationType?> GetByIdAsync(IdentificationTypeId id, CancellationToken cancellationToken = default)
    {
        return await _context.IdentificationTypes.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IEnumerable<IdentificationType>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.IdentificationTypes.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<IdentificationType>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        return await _context.IdentificationTypes
            .Where(it => it.IsActive)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(IdentificationType identificationType, CancellationToken cancellationToken = default)
    {
        await _context.IdentificationTypes.AddAsync(identificationType, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(IdentificationType identificationType, CancellationToken cancellationToken = default)
    {
        _context.IdentificationTypes.Update(identificationType);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(IdentificationType identificationType, CancellationToken cancellationToken = default)
    {
        _context.IdentificationTypes.Remove(identificationType);
        await _context.SaveChangesAsync(cancellationToken);
    }

    // Note: SaveChangesAsync is not needed since we're saving in each operation
    // This is kept for interface compatibility
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}