// IdentityManagement
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.

using OroKernel.Shared.Entities;
using OroKernel.Shared.Interfaces;

namespace IdentityManagement;

/// <summary>
/// Represents an identification type in the system using BaseEntity<T, TId>.
/// This demonstrates the use of BaseEntity<IdentificationType, int> where
/// IdentificationType is the entity type and int is the identifier type.
/// </summary>
public class IdentificationType : BaseEntity<IdentificationType, int>, IAggregateRoot
{
    /// <summary>
    /// Gets or sets the name of the identification type.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the identification type.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the country code for this identification type.
    /// </summary>
    public string CountryCode { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether this identification type is active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets or sets the maximum length allowed for this identification type.
    /// </summary>
    public int MaxLength { get; set; }

    /// <summary>
    /// Gets or sets the validation pattern (regex) for this identification type.
    /// </summary>
    public string ValidationPattern { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date when this identification type was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Initializes a new instance of the IdentificationType class with a custom ID.
    /// </summary>
    /// <param name="id">The custom identification type identifier.</param>
    public IdentificationType(int id)
    {
        Id = id;
    }

    /// <summary>
    /// Initializes a new instance of the IdentificationType class.
    /// </summary>
    public IdentificationType() { }

    /// <summary>
    /// Updates the identification type information.
    /// </summary>
    /// <param name="name">The new name.</param>
    /// <param name="description">The new description.</param>
    /// <param name="maxLength">The new maximum length.</param>
    public void UpdateInfo(string name, string description, int maxLength)
    {
        Name = name;
        Description = description;
        MaxLength = maxLength;
    }

    /// <summary>
    /// Sets the validation pattern for this identification type.
    /// </summary>
    /// <param name="pattern">The regex validation pattern.</param>
    public void SetValidationPattern(string pattern)
    {
        ValidationPattern = pattern;
    }

    /// <summary>
    /// Deactivates this identification type.
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
    }

    /// <summary>
    /// Activates this identification type.
    /// </summary>
    public void Activate()
    {
        IsActive = true;
    }

    /// <summary>
    /// Validates if a given identification number matches this type's pattern.
    /// </summary>
    /// <param name="identificationNumber">The identification number to validate.</param>
    /// <returns>True if valid, false otherwise.</returns>
    public bool ValidateIdentificationNumber(string identificationNumber)
    {
        if (string.IsNullOrEmpty(identificationNumber))
            return false;

        if (identificationNumber.Length > MaxLength)
            return false;

        if (!string.IsNullOrEmpty(ValidationPattern))
        {
            return System.Text.RegularExpressions.Regex.IsMatch(identificationNumber, ValidationPattern);
        }

        return true;
    }
}