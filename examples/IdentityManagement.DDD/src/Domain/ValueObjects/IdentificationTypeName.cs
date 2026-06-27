// IdentityManagement.DDD
// Copyright (C) 2026 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.

namespace IdentityManagement.DDD.Domain.ValueObjects;

/// <summary>
/// Identification type name value object with validation, modeled as a positional record.
/// </summary>
public sealed record IdentificationTypeName(string Value)
{
    /// <summary>
    /// Factory that validates and trims the name value.
    /// </summary>
    public static IdentificationTypeName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Name cannot be null or empty", nameof(value));

        if (value.Length > 100)
            throw new ArgumentException("Name cannot be longer than 100 characters", nameof(value));

        return new IdentificationTypeName(value.Trim());
    }

    /// <summary>
    /// Returns the string representation of the name.
    /// </summary>
    public override string ToString() => Value;

    /// <summary>
    /// Implicit conversion to string.
    /// </summary>
    public static implicit operator string(IdentificationTypeName name) => name.Value;

    /// <summary>
    /// Explicit conversion from string.
    /// </summary>
    public static explicit operator IdentificationTypeName(string value) => Create(value);
}
