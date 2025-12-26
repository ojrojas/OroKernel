// CountryCode.cs - Value Object
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.

using OroKernel.Shared.Entities;

namespace IdentityManagement.DDD.Domain.ValueObjects;

/// <summary>
/// Country code value object with validation (ISO 3166-1 alpha-2 or alpha-3)
/// </summary>
public sealed class CountryCode : BaseValueObject
{
    /// <summary>
    /// Gets the country code value.
    /// </summary>
    public string Value { get; private set; }

    /// <summary>
    /// Creates a new country code.
    /// </summary>
    public CountryCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Country code cannot be null or empty", nameof(value));

        var normalized = value.Trim().ToUpperInvariant();

        if (normalized.Length != 2 && normalized.Length != 3)
            throw new ArgumentException("Country code must be 2 or 3 characters long", nameof(value));

        if (!System.Text.RegularExpressions.Regex.IsMatch(normalized, @"^[A-Z]+$"))
            throw new ArgumentException("Country code can only contain letters", nameof(value));

        Value = normalized;
    }

    /// <summary>
    /// Creates a new country code (factory method).
    /// </summary>
    public static CountryCode Create(string value) => new(value);

    /// <summary>
    /// Returns the string representation of the country code.
    /// </summary>
    public override string ToString() => Value;

    /// <summary>
    /// Implicit conversion to string.
    /// </summary>
    public static implicit operator string(CountryCode code) => code.Value;

    /// <summary>
    /// Explicit conversion from string.
    /// </summary>
    public static explicit operator CountryCode(string value) => new(value);

    protected override IEnumerable<object?> GetEquatibilityComponents()
    {
        yield return Value;
    }
}