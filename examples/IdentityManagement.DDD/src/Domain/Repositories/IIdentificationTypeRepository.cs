// IIdentificationTypeRepository.cs - Domain Repository Interface
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.

using IdentityManagement.DDD.Domain.Entities;

namespace IdentityManagement.DDD.Domain.Repositories;

/// <summary>
/// Repository interface for IdentificationType aggregate
/// </summary>
public interface IIdentificationTypeRepository
{
    /// <summary>
    /// Gets an identification type by ID.
    /// </summary>
    Task<IdentificationType?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all identification types.
    /// </summary>
    Task<IEnumerable<IdentificationType>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all active identification types.
    /// </summary>
    Task<IEnumerable<IdentificationType>> GetActiveAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new identification type.
    /// </summary>
    Task AddAsync(IdentificationType identificationType, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing identification type.
    /// </summary>
    Task UpdateAsync(IdentificationType identificationType, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an identification type.
    /// </summary>
    Task DeleteAsync(IdentificationType identificationType, CancellationToken cancellationToken = default);

    /// <summary>
    /// Saves all changes.
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}