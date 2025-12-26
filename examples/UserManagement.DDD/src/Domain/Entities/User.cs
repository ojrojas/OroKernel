// User.cs - Domain Entity
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.

using OroKernel.Shared.Entities;
using OroKernel.Shared.Interfaces;
using UserManagement.DDD.Domain.ValueObjects;

namespace UserManagement.DDD.Domain.Entities;

/// <summary>
/// User domain entity following DDD principles
/// </summary>
public class User : BaseEntity, IAggregateRoot
{
    /// <summary>
    /// Gets the username.
    /// </summary>
    public UserName UserName { get; private set; } = null!;

    /// <summary>
    /// Gets the full name.
    /// </summary>
    public FullName FullName { get; private set; } = null!;

    /// <summary>
    /// Gets the email address.
    /// </summary>
    public Email Email { get; private set; } = null!;

    /// <summary>
    /// Gets or sets whether the user is active.
    /// </summary>
    public bool IsActive { get; private set; } = true;

    /// <summary>
    /// Gets the date when the user was created.
    /// </summary>
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets the date when the user was last modified.
    /// </summary>
    public DateTime? LastModifiedAt { get; private set; }

    // Private constructor for EF Core
    private User() { }

    /// <summary>
    /// Creates a new user with a custom ID.
    /// </summary>
    public User(Guid id, UserName userName, FullName fullName, Email email)
    {
        Id = id;
        UserName = userName ?? throw new ArgumentNullException(nameof(userName));
        FullName = fullName ?? throw new ArgumentNullException(nameof(fullName));
        Email = email ?? throw new ArgumentNullException(nameof(email));
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Creates a new user with auto-generated ID.
    /// </summary>
    public User(UserName userName, FullName fullName, Email email)
        : this(Guid.CreateVersion7(), userName, fullName, email)
    {
    }

    /// <summary>
    /// Updates the user's information.
    /// </summary>
    public void UpdateInfo(FullName newFullName, Email newEmail)
    {
        FullName = newFullName ?? throw new ArgumentNullException(nameof(newFullName));
        Email = newEmail ?? throw new ArgumentNullException(nameof(newEmail));
        LastModifiedAt = DateTime.UtcNow;

        // Domain event could be raised here
        // AddDomainEvent(new UserUpdatedEvent(Id, FullName, Email));
    }

    /// <summary>
    /// Deactivates the user.
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
        LastModifiedAt = DateTime.UtcNow;

        // Domain event could be raised here
        // AddDomainEvent(new UserDeactivatedEvent(Id));
    }

    /// <summary>
    /// Activates the user.
    /// </summary>
    public void Activate()
    {
        IsActive = true;
        LastModifiedAt = DateTime.UtcNow;

        // Domain event could be raised here
        // AddDomainEvent(new UserActivatedEvent(Id));
    }

    /// <summary>
    /// Changes the username.
    /// </summary>
    public void ChangeUserName(UserName newUserName)
    {
        UserName = newUserName ?? throw new ArgumentNullException(nameof(newUserName));
        LastModifiedAt = DateTime.UtcNow;

        // Domain event could be raised here
        // AddDomainEvent(new UserNameChangedEvent(Id, UserName));
    }
}