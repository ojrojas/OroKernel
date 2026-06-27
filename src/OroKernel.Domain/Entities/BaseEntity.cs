// OroKernel
// Copyright (C) 2026 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.
namespace OroKernel.Domain.Entities;

/// <summary>
/// Base entity with a Guid identifier
/// </summary>
public abstract class BaseEntity : WithDomainEventBase
{
    /// <summary>
    /// Unique identifier
    /// </summary>
    public Guid Id { get; set; } = Guid.CreateVersion7();
}

/// <summary>
/// Base entity with a typed identifier
/// </summary>
/// <typeparam name="TId">Tid Identifier</typeparam>
public abstract class BaseEntity<TId> : WithDomainEventBase where TId : struct, IEquatable<TId>
{
    /// <summary>
    /// Unique identifier
    /// </summary>
    public TId Id { get; set; } = default!;
}

/// <summary>
/// Base entity with a typed identifier and self-referencing generic type
/// </summary>
/// <typeparam name="T">T entity type</typeparam>
/// <typeparam name="TId">TId identifier</typeparam>
public abstract class BaseEntity<T, TId> : WithDomainEventBase where T : BaseEntity<T, TId>
{
    /// <summary>
    /// Unique identifier
    /// </summary>
    public TId Id { get; set; } = default!;

    public bool Equals(BaseEntity<T, TId>? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        if (GetType() != other.GetType()) return false;
        if (EqualityComparer<TId>.Default.Equals(Id, default)) return false;
        return EqualityComparer<TId>.Default.Equals(Id, other.Id);
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current object
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj) =>
        obj is BaseEntity<T, TId> other && Equals(other);

    /// <summary>
    /// Server as the default hash function
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() =>
        EqualityComparer<TId>.Default.Equals(Id, default)
            ? base.GetHashCode()
            : HashCode.Combine(GetType(), Id);


    /// <summary>
    /// Equality operator overload
    /// </summary>
    /// <param name="left">object type T left</param>
    /// <param name="right">object type T right</param>
    /// <returns>True as equals</returns>
    public static bool operator ==(BaseEntity<T, TId>? left, BaseEntity<T, TId>? right) =>
        left?.Equals(right) ?? right is null;

    /// <summary>
    /// Inequality operator overload
    /// </summary>
    /// <param name="left">object type T left</param>
    /// <param name="right">object type T right</param>
    /// <returns>True as no equal</returns>
    public static bool operator !=(BaseEntity<T, TId>? left, BaseEntity<T, TId>? right) =>
        !(left == right);
}
