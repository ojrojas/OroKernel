// IdentityManagement.DDD
// Copyright (C) 2026 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.

namespace IdentityManagement.DDD.Domain.ValueObjects;

/// <summary>
/// Validation pattern value object for regex patterns, modeled as a positional record.
/// </summary>
public sealed record ValidationPattern(string Value)
{
    /// <summary>
    /// Factory that validates the regex pattern.
    /// </summary>
    public static ValidationPattern Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Validation pattern cannot be null or empty", nameof(value));

        if (value.Length > 500)
            throw new ArgumentException("Validation pattern cannot be longer than 500 characters", nameof(value));

        try
        {
            System.Text.RegularExpressions.Regex.Match("", value);
        }
        catch (ArgumentException ex)
        {
            throw new ArgumentException($"Invalid regex pattern: {ex.Message}", nameof(value));
        }

        return new ValidationPattern(value);
    }

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
    public static explicit operator ValidationPattern(string value) => Create(value);
}
