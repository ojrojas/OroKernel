// CreateUserCommand.cs - Application Command
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.

using UserManagement.DDD.Domain.ValueObjects;

namespace UserManagement.DDD.Application.Commands;

/// <summary>
/// Command to create a new user
/// </summary>
public record CreateUserCommand(
    Guid? Id,
    string UserName,
    string FirstName,
    string LastName,
    string Email
);