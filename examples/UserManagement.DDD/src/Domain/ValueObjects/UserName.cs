// UserManagement.DDD
// Copyright (C) 2026 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.

namespace UserManagement.DDD.Domain.ValueObjects;

/// <summary>
/// Username value object with validation, modeled as a positional record.
/// </summary>
public sealed record UserName(string Value)
{
    private static readonly System.Text.RegularExpressions.Regex UserNamePattern = new(
        @"^[a-zA-Z0-9_]+$",
        System.Text.RegularExpressions.RegexOptions.Compiled);

    /// <summary>
    /// Factory that validates the username value.
    /// </summary>
    public static UserName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Username cannot be null or empty", nameof(value));

        if (value.Length < 3)
            throw new ArgumentException("Username must be at least 3 characters long", nameof(value));

        if (value.Length > 50)
            throw new ArgumentException("Username cannot be longer than 50 characters", nameof(value));

        if (!UserNamePattern.IsMatch(value))
            throw new ArgumentException("Username can only contain letters, numbers, and underscores", nameof(value));

        return new UserName(value);
    }

    /// <summary>
    /// Returns the string representation of the username.
    /// </summary>
    public override string ToString() => Value;

    /// <summary>
    /// Implicit conversion to string.
    /// </summary>
    public static implicit operator string(UserName userName) => userName.Value;

    /// <summary>
    /// Explicit conversion from string.
    /// </summary>
    public static explicit operator UserName(string value) => Create(value);
}
