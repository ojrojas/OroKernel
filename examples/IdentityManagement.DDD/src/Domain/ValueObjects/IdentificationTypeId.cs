// IdentificationTypeId.cs - Value Object for Entity ID
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.

using OroKernel.Shared.Entities;

namespace IdentityManagement.DDD.Domain.ValueObjects;

/// <summary>
/// Identification type ID value object
/// </summary>
public sealed class IdentificationTypeId : BaseValueObject
{
    /// <summary>
    /// Gets the Guid value.
    /// </summary>
    public Guid Value { get; private set; }

    /// <summary>
    /// Creates a new identification type ID.
    /// </summary>
    public IdentificationTypeId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("ID cannot be empty", nameof(value));

        Value = value;
    }

    /// <summary>
    /// Creates a new identification type ID with a new Guid.
    /// </summary>
    public static IdentificationTypeId NewId() => new(Guid.NewGuid());

    /// <summary>
    /// Creates a identification type ID from string.
    /// </summary>
    public static IdentificationTypeId FromString(string value) => new(Guid.Parse(value));

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
    public static explicit operator IdentificationTypeId(Guid value) => new(value);

    protected override IEnumerable<object?> GetEquatibilityComponents()
    {
        yield return Value;
    }
}