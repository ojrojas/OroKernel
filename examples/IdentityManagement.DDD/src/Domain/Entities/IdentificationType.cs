// IdentificationType.cs - Domain Entity
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.

using OroKernel.Shared.Entities;
using OroKernel.Shared.Interfaces;
using IdentityManagement.DDD.Domain.ValueObjects;

namespace IdentityManagement.DDD.Domain.Entities;

/// <summary>
/// Identification type domain entity following DDD principles
/// </summary>
public class IdentificationType : BaseEntity<IdentificationType, IdentificationTypeId>, IAggregateRoot
{
    /// <summary>
    /// Gets the name of the identification type.
    /// </summary>
    public IdentificationTypeName Name { get; private set; } = null!;

    /// <summary>
    /// Gets the description.
    /// </summary>
    public string Description { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the country code.
    /// </summary>
    public CountryCode CountryCode { get; private set; } = null!;

    /// <summary>
    /// Gets the maximum length allowed.
    /// </summary>
    public int MaxLength { get; private set; }

    /// <summary>
    /// Gets the validation pattern.
    /// </summary>
    public ValidationPattern ValidationPattern { get; private set; } = null!;

    /// <summary>
    /// Gets or sets whether this identification type is active.
    /// </summary>
    public bool IsActive { get; private set; } = true;

    /// <summary>
    /// Gets the date when this type was created.
    /// </summary>
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets the date when this type was last modified.
    /// </summary>
    public DateTime? LastModifiedAt { get; private set; }

    // Private constructor for EF Core
    private IdentificationType() { }

    /// <summary>
    /// Creates a new identification type.
    /// </summary>
    public static IdentificationType Create(IdentificationTypeName name, string description,
        CountryCode countryCode, int maxLength, ValidationPattern validationPattern)
    {
        return new IdentificationType(IdentificationTypeId.NewId(), name, description, countryCode, maxLength, validationPattern);
    }

    /// <summary>
    /// Creates a new identification type with a custom ID.
    /// </summary>
    public IdentificationType(IdentificationTypeId id, IdentificationTypeName name, string description,
        CountryCode countryCode, int maxLength, ValidationPattern validationPattern)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        CountryCode = countryCode ?? throw new ArgumentNullException(nameof(countryCode));
        MaxLength = maxLength;
        ValidationPattern = validationPattern ?? throw new ArgumentNullException(nameof(validationPattern));
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the identification type information.
    /// </summary>
    public void Update(IdentificationTypeName newName, string newDescription, CountryCode newCountryCode, int newMaxLength, ValidationPattern newValidationPattern)
    {
        Name = newName ?? throw new ArgumentNullException(nameof(newName));
        Description = newDescription ?? throw new ArgumentNullException(nameof(newDescription));
        CountryCode = newCountryCode ?? throw new ArgumentNullException(nameof(newCountryCode));
        MaxLength = newMaxLength;
        ValidationPattern = newValidationPattern ?? throw new ArgumentNullException(nameof(newValidationPattern));
        LastModifiedAt = DateTime.UtcNow;

        // Domain event could be raised here
        // AddDomainEvent(new IdentificationTypeUpdatedEvent(Id, Name, Description));
    }

    /// <summary>
    /// Deactivates the identification type.
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
        LastModifiedAt = DateTime.UtcNow;

        // Domain event could be raised here
        // AddDomainEvent(new IdentificationTypeDeactivatedEvent(Id));
    }

    /// <summary>
    /// Activates the identification type.
    /// </summary>
    public void Activate()
    {
        IsActive = true;
        LastModifiedAt = DateTime.UtcNow;

        // Domain event could be raised here
        // AddDomainEvent(new IdentificationTypeActivatedEvent(Id));
    }

    /// <summary>
    /// Validates an identification number against this type's pattern.
    /// </summary>
    public bool ValidateIdentificationNumber(string identificationNumber)
    {
        if (string.IsNullOrWhiteSpace(identificationNumber))
            return false;

        return System.Text.RegularExpressions.Regex.IsMatch(
            identificationNumber, ValidationPattern.Value);
    }
}