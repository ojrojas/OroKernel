// UpdateUserCommand.cs - Application Command
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.

namespace UserManagement.DDD.Application.Commands;

/// <summary>
/// Command to update user information
/// </summary>
public record UpdateUserCommand(
    Guid UserId,
    string FirstName,
    string LastName,
    string Email
);