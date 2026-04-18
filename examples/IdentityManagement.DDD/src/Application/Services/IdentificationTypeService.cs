// IdentificationTypeService.cs - Application Service
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.

using IdentityManagement.DDD.Application.Commands;
using IdentityManagement.DDD.Application.DTOs;
using IdentityManagement.DDD.Application.Queries;
using IdentityManagement.DDD.Domain.Entities;
using IdentityManagement.DDD.Domain.Repositories;
using IdentityManagement.DDD.Domain.ValueObjects;

namespace IdentityManagement.DDD.Application.Services;

/// <summary>
/// Application service for IdentificationType operations
/// </summary>
public class IdentificationTypeService
{
    private readonly IIdentificationTypeRepository _repository;

    public IdentificationTypeService(IIdentificationTypeRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Creates a new identification type
    /// </summary>
    public async Task<IdentificationTypeDto> CreateIdentificationTypeAsync(CreateIdentificationTypeCommand command)
    {
        var name = IdentificationTypeName.Create(command.Name);
        var countryCode = CountryCode.Create(command.CountryCode);
        var pattern = ValidationPattern.CreatePattern(command.ValidationPattern);

        var identificationType = IdentificationType.Create(
            name,
            command.Description,
            countryCode,
            command.MaxLength,
            pattern
        );

        await _repository.AddAsync(identificationType);

        return MapToDto(identificationType);
    }

    /// <summary>
    /// Updates an existing identification type
    /// </summary>
    public async Task<IdentificationTypeDto> UpdateIdentificationTypeAsync(UpdateIdentificationTypeCommand command)
    {
        var identificationType = await _repository.GetByIdAsync(command.Id);
        if (identificationType == null)
            throw new InvalidOperationException($"IdentificationType with ID {command.Id} not found.");

        var name = IdentificationTypeName.Create(command.Name);
        var countryCode = CountryCode.Create(command.CountryCode);
        var validationPattern = ValidationPattern.CreatePattern(command.ValidationPattern);

        identificationType.Update(
            name,
            command.Description,
            countryCode,
            command.MaxLength,
            validationPattern
        );

        await _repository.UpdateAsync(identificationType);

        return MapToDto(identificationType);
    }

    /// <summary>
    /// Deactivates an identification type
    /// </summary>
    public async Task DeactivateIdentificationTypeAsync(DeactivateIdentificationTypeCommand command)
    {
        var identificationType = await _repository.GetByIdAsync(command.Id);
        if (identificationType == null)
            throw new InvalidOperationException($"IdentificationType with ID {command.Id} not found.");

        identificationType.Deactivate();

        await _repository.UpdateAsync(identificationType);
    }

    /// <summary>
    /// Gets an identification type by ID
    /// </summary>
    public async Task<IdentificationTypeDto?> GetIdentificationTypeByIdAsync(GetIdentificationTypeByIdQuery query)
    {
        var identificationType = await _repository.GetByIdAsync(query.Id);
        return identificationType != null ? MapToDto(identificationType) : null;
    }

    /// <summary>
    /// Gets all identification types
    /// </summary>
    public async Task<IEnumerable<IdentificationTypeDto>> GetAllIdentificationTypesAsync(GetAllIdentificationTypesQuery query)
    {
        var identificationTypes = await _repository.GetAllAsync();
        return identificationTypes.Select(MapToDto);
    }

    /// <summary>
    /// Gets active identification types
    /// </summary>
    public async Task<IEnumerable<IdentificationTypeDto>> GetActiveIdentificationTypesAsync()
    {
        var identificationTypes = await _repository.GetActiveAsync();
        return identificationTypes.Select(MapToDto);
    }

    private static IdentificationTypeDto MapToDto(IdentificationType identificationType)
    {
        return new IdentificationTypeDto(
            identificationType.Id.Value,
            identificationType.Name.Value,
            identificationType.Description,
            identificationType.CountryCode.Value,
            identificationType.MaxLength,
            identificationType.ValidationPattern.Value,
            identificationType.IsActive,
            identificationType.CreatedAt,
            identificationType.LastModifiedAt
        );
    }
}