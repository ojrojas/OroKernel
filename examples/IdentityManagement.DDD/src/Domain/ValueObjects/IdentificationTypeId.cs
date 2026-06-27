// IdentityManagement.DDD
// Copyright (C) 2026 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.

namespace IdentityManagement.DDD.Domain.ValueObjects;

/// <summary>
/// Identification type ID value object, modeled as a positional record wrapping a Guid.
/// </summary>
public sealed record IdentificationTypeId(Guid Value)
{
    /// <summary>
    /// Factory that validates the Guid is not empty.
    /// </summary>
    public static IdentificationTypeId Create(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("ID cannot be empty", nameof(value));

        return new IdentificationTypeId(value);
    }

    /// <summary>
    /// Creates a new identification type ID with a fresh Guid.
    /// </summary>
    public static IdentificationTypeId NewId() => new(Guid.NewGuid());

    /// <summary>
    /// Creates an identification type ID from its string representation.
    /// </summary>
    public static IdentificationTypeId FromString(string value) => Create(Guid.Parse(value));

    /// <summary>
    /// Returns the string representation of the ID.
    /// </summary>
    public override string ToString() => Value.ToString();

    /// <summary>
    /// Implicit conversion to Guid.
    /// </summary>
    public static implicit operator Guid(IdentificationTypeId id) => id.Value;

    /// <summary>
    /// Explicit conversion from Guid.
    /// </summary>
    public static explicit operator IdentificationTypeId(Guid value) => Create(value);
}
