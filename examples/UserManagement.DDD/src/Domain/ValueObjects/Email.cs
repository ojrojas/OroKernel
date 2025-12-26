// Email.cs - Value Object
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.

using OroKernel.Shared.Entities;

namespace UserManagement.DDD.Domain.ValueObjects;

/// <summary>
/// Email address value object with validation
/// </summary>
public sealed class Email : BaseValueObject
{
    /// <summary>
    /// Gets the email address value.
    /// </summary>
    public string Value { get; private set; }

    /// <summary>
    /// Creates a new email address.
    /// </summary>
    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email cannot be null or empty", nameof(value));

        if (value.Length > 255)
            throw new ArgumentException("Email cannot be longer than 255 characters", nameof(value));

        // Basic email validation regex
        if (!System.Text.RegularExpressions.Regex.IsMatch(value,
            @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$"))
            throw new ArgumentException("Invalid email format", nameof(value));

        Value = value.ToLowerInvariant().Trim();
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
    public static explicit operator Email(string value) => new(value);

    protected override IEnumerable<object?> GetEquatibilityComponents()
    {
        yield return Value.ToLowerInvariant();
    }
}