// IdentificationTypeName.cs - Value Object
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.

using OroKernel.Shared.Entities;

namespace IdentityManagement.DDD.Domain.ValueObjects;

/// <summary>
/// Identification type name value object with validation
/// </summary>
public sealed class IdentificationTypeName : BaseValueObject
{
    /// <summary>
    /// Gets the name value.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Creates a new identification type name.
    /// </summary>
    public IdentificationTypeName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Name cannot be null or empty", nameof(value));

        if (value.Length > 100)
            throw new ArgumentException("Name cannot be longer than 100 characters", nameof(value));

        Value = value.Trim();
    }

    /// <summary>
    /// Creates a new identification type name (factory method).
    /// </summary>
    public static IdentificationTypeName Create(string value) => new(value);

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
    public static explicit operator IdentificationTypeName(string value) => new(value);

    protected override IEnumerable<object?> GetEquatibilityComponents()
    {
        yield return Value.ToLowerInvariant();
    }
}