// OroKernel
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.
namespace OroKernel.Shared.Entities;

/// <summary>
/// Base class for value objects
/// </summary>
[Serializable]
public abstract class BaseValueObject : IEquatable<BaseValueObject>
{
    /// <summary>
    /// Gets the components that define the equality of the value object
    /// </summary>
    /// <returns></returns>
    protected abstract IEnumerable<object?> GetEquatibilityComponents();

    /// <summary>
    /// Checks equality between two value objects
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        return GetEquatibilityComponents().Aggregate(1, HashCode.Combine);
    }

    /// <summary>
    /// Checks equality between two value objects
    /// </summary>
    public bool Equals(BaseValueObject? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        if (GetType() != other.GetType()) return false;

        return GetEquatibilityComponents().SequenceEqual(other.GetEquatibilityComponents());
    }

    /// <summary>
    /// Checks equality with another object
    /// </summary>
    public override bool Equals(object? obj)
    {
        return Equals(obj as BaseValueObject);
    }
}
