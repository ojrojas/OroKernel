// FullName.cs - Value Object
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.

using OroKernel.Shared.Entities;

namespace UserManagement.DDD.Domain.ValueObjects;

/// <summary>
/// Full name value object composed of first and last name
/// </summary>
public sealed class FullName : BaseValueObject
{
    /// <summary>
    /// Gets the first name.
    /// </summary>
    public string FirstName { get; private set; }

    /// <summary>
    /// Gets the last name.
    /// </summary>
    public string LastName { get; private set; }

    /// <summary>
    /// Gets the full name as a single string.
    /// </summary>
    public string DisplayName => $"{FirstName} {LastName}";

    /// <summary>
    /// Creates a new full name.
    /// </summary>
    public FullName(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be null or empty", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be null or empty", nameof(lastName));

        if (firstName.Length > 100)
            throw new ArgumentException("First name cannot be longer than 100 characters", nameof(firstName));

        if (lastName.Length > 100)
            throw new ArgumentException("Last name cannot be longer than 100 characters", nameof(lastName));

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
    }

    /// <summary>
    /// Returns the string representation of the full name.
    /// </summary>
    public override string ToString() => DisplayName;

    /// <summary>
    /// Implicit conversion to string.
    /// </summary>
    public static implicit operator string(FullName fullName) => fullName.DisplayName;

    protected override IEnumerable<object?> GetEquatibilityComponents()
    {
        yield return FirstName.ToLowerInvariant();
        yield return LastName.ToLowerInvariant();
    }
}