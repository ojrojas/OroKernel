// UserManagement.DDD
// Copyright (C) 2026 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.

namespace UserManagement.DDD.Domain.ValueObjects;

/// <summary>
/// Full name value object composed of first and last name, modeled as a positional record.
/// </summary>
public sealed record FullName(string FirstName, string LastName)
{
    public string DisplayName => $"{FirstName} {LastName}";

    /// <summary>
    /// Factory that validates and trims the first and last name values.
    /// </summary>
    public static FullName Create(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be null or empty", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be null or empty", nameof(lastName));

        if (firstName.Length > 100)
            throw new ArgumentException("First name cannot be longer than 100 characters", nameof(firstName));

        if (lastName.Length > 100)
            throw new ArgumentException("Last name cannot be longer than 100 characters", nameof(lastName));

        return new FullName(firstName.Trim(), lastName.Trim());
    }

    /// <summary>
    /// Returns the string representation of the full name.
    /// </summary>
    public override string ToString() => DisplayName;

    /// <summary>
    /// Implicit conversion to string.
    /// </summary>
    public static implicit operator string(FullName fullName) => fullName.DisplayName;
}
