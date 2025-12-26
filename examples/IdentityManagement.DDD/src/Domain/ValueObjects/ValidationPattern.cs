// ValidationPattern.cs - Value Object
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.

using OroKernel.Shared.Entities;

namespace IdentityManagement.DDD.Domain.ValueObjects;

/// <summary>
/// Validation pattern value object for regex patterns
/// </summary>
public sealed class ValidationPattern : BaseValueObject
{
    /// <summary>
    /// Gets the regex pattern value.
    /// </summary>
    public string Value { get; private set; }

    /// <summary>
    /// Creates a new validation pattern.
    /// </summary>
    public ValidationPattern(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Validation pattern cannot be null or empty", nameof(value));

        if (value.Length > 500)
            throw new ArgumentException("Validation pattern cannot be longer than 500 characters", nameof(value));

        // Try to validate the regex pattern
        try
        {
            System.Text.RegularExpressions.Regex.Match("", value);
        }
        catch (ArgumentException ex)
        {
            throw new ArgumentException($"Invalid regex pattern: {ex.Message}", nameof(value));
        }

        Value = value;
    }

    /// <summary>
    /// Creates a new validation pattern (factory method).
    /// </summary>
    public static ValidationPattern CreatePattern(string value) => new(value);

    /// <summary>
    /// Returns the string representation of the pattern.
    /// </summary>
    public override string ToString() => Value;

    /// <summary>
    /// Implicit conversion to string.
    /// </summary>
    public static implicit operator string(ValidationPattern pattern) => pattern.Value;

    /// <summary>
    /// Explicit conversion from string.
    /// </summary>
    public static explicit operator ValidationPattern(string value) => new(value);

    protected override IEnumerable<object?> GetEquatibilityComponents()
    {
        yield return Value;
    }
}