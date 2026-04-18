// DeactivateUserCommand.cs - Application Command
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.

namespace UserManagement.DDD.Application.Commands;

/// <summary>
/// Command to deactivate a user
/// </summary>
public record DeactivateUserCommand(Guid UserId);