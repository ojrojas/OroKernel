// UserManagement.DDD
// Copyright (C) 2026 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.

namespace UserManagement.DDD.Domain.ValueObjects;

/// <summary>
/// Email address value object with validation, modeled as a positional record.
/// Equality, hash and immutability are intrinsic to <see cref="record"/> in .NET 10/11.
/// </summary>
public sealed record Email(string Value)
{
    private static readonly System.Text.RegularExpressions.Regex EmailPattern = new(
        @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$",
        System.Text.RegularExpressions.RegexOptions.Compiled);

    /// <summary>
    /// Factory that validates and normalizes the email value.
    /// </summary>
    public static Email Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email cannot be null or empty", nameof(value));

        if (value.Length > 255)
            throw new ArgumentException("Email cannot be longer than 255 characters", nameof(value));

        var normalized = value.Trim().ToLowerInvariant();

        if (!EmailPattern.IsMatch(normalized))
            throw new ArgumentException("Invalid email format", nameof(value));

        return new Email(normalized);
    }

    /// <summary>
    /// Returns the string representation of the email.
    /// </summary>
    public override string ToString() => Value;

    /// <summary>
    /// Implicit conversion to string.
    /// </summary>
    public static implicit operator string(Email email) => email.Value;

    /// <summary>
    /// Explicit conversion from string.
    /// </summary>
    public static explicit operator Email(string value) => Create(value);
}
