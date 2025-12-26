// UserName.cs - Value Object
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.

using OroKernel.Shared.Entities;

namespace UserManagement.DDD.Domain.ValueObjects;

/// <summary>
/// Username value object with validation
/// </summary>
public sealed class UserName : BaseValueObject
{
    /// <summary>
    /// Gets the username value.
    /// </summary>
    public string Value { get; private set; }

    /// <summary>
    /// Creates a new username.
    /// </summary>
    public UserName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Username cannot be null or empty", nameof(value));

        if (value.Length < 3)
            throw new ArgumentException("Username must be at least 3 characters long", nameof(value));

        if (value.Length > 50)
            throw new ArgumentException("Username cannot be longer than 50 characters", nameof(value));

        if (!System.Text.RegularExpressions.Regex.IsMatch(value, @"^[a-zA-Z0-9_]+$"))
            throw new ArgumentException("Username can only contain letters, numbers, and underscores", nameof(value));

        Value = value;
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
    public static explicit operator UserName(string value) => new(value);

    protected override IEnumerable<object?> GetEquatibilityComponents()
    {
        yield return Value.ToLowerInvariant();
    }
}