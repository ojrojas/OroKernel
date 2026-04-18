// UserApplicationService.cs - Application Service
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.

using UserManagement.DDD.Application.Commands;
using UserManagement.DDD.Application.DTOs;
using UserManagement.DDD.Application.Queries;
using UserManagement.DDD.Domain.Entities;
using UserManagement.DDD.Domain.Repositories;
using UserManagement.DDD.Domain.ValueObjects;

namespace UserManagement.DDD.Application.Services;

/// <summary>
/// Application service for user operations
/// </summary>
public class UserApplicationService
{
    private readonly IUserRepository _userRepository;

    public UserApplicationService(IUserRepository userRepository)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    /// <summary>
    /// Creates a new user.
    /// </summary>
    public async Task<UserDto> CreateUserAsync(CreateUserCommand command, CancellationToken cancellationToken = default)
    {
        // Validate business rules
        var existingUser = await _userRepository.GetByUserNameAsync(command.UserName, cancellationToken);
        if (existingUser != null)
            throw new InvalidOperationException($"Username '{command.UserName}' is already taken.");

        var existingEmail = await _userRepository.GetByEmailAsync(command.Email, cancellationToken);
        if (existingEmail != null)
            throw new InvalidOperationException($"Email '{command.Email}' is already registered.");

        // Create value objects
        var userName = new UserName(command.UserName);
        var fullName = new FullName(command.FirstName, command.LastName);
        var email = new Email(command.Email);

        // Create entity
        User user;
        if (command.Id.HasValue)
        {
            user = new User(command.Id.Value, userName, fullName, email);
        }
        else
        {
            user = new User(userName, fullName, email);
        }

        // Save to repository
        await _userRepository.AddAsync(user, cancellationToken);
        await _userRepository.SaveChangesAsync(cancellationToken);

        return MapToDto(user);
    }

    /// <summary>
    /// Updates user information.
    /// </summary>
    public async Task<UserDto> UpdateUserAsync(UpdateUserCommand command, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(command.UserId, cancellationToken);
        if (user == null)
            throw new InvalidOperationException($"User with ID '{command.UserId}' not found.");

        // Check if email is already taken by another user
        var existingEmail = await _userRepository.GetByEmailAsync(command.Email, cancellationToken);
        if (existingEmail != null && existingEmail.Id != command.UserId)
            throw new InvalidOperationException($"Email '{command.Email}' is already registered.");

        // Update user
        var fullName = new FullName(command.FirstName, command.LastName);
        var email = new Email(command.Email);

        user.UpdateInfo(fullName, email);

        await _userRepository.UpdateAsync(user, cancellationToken);
        await _userRepository.SaveChangesAsync(cancellationToken);

        return MapToDto(user);
    }

    /// <summary>
    /// Deactivates a user.
    /// </summary>
    public async Task DeactivateUserAsync(DeactivateUserCommand command, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(command.UserId, cancellationToken);
        if (user == null)
            throw new InvalidOperationException($"User with ID '{command.UserId}' not found.");

        user.Deactivate();

        await _userRepository.UpdateAsync(user, cancellationToken);
        await _userRepository.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Activates a user.
    /// </summary>
    public async Task ActivateUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user == null)
            throw new InvalidOperationException($"User with ID '{userId}' not found.");

        user.Activate();

        await _userRepository.UpdateAsync(user, cancellationToken);
        await _userRepository.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Gets a user by ID.
    /// </summary>
    public async Task<UserDto?> GetUserByIdAsync(GetUserByIdQuery query, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(query.UserId, cancellationToken);
        return user != null ? MapToDto(user) : null;
    }

    /// <summary>
    /// Gets all users.
    /// </summary>
    public async Task<IEnumerable<UserDto>> GetAllUsersAsync(GetAllUsersQuery query, CancellationToken cancellationToken = default)
    {
        var users = await _userRepository.GetAllAsync(cancellationToken);
        return users.Select(MapToDto);
    }

    /// <summary>
    /// Deletes a user.
    /// </summary>
    public async Task DeleteUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user == null)
            throw new InvalidOperationException($"User with ID '{userId}' not found.");

        await _userRepository.DeleteAsync(user, cancellationToken);
        await _userRepository.SaveChangesAsync(cancellationToken);
    }

    private static UserDto MapToDto(User user)
    {
        return new UserDto(
            user.Id,
            user.UserName,
            user.FullName.FirstName,
            user.FullName.LastName,
            user.FullName.DisplayName,
            user.Email,
            user.IsActive,
            user.CreatedAt,
            user.LastModifiedAt
        );
    }
}