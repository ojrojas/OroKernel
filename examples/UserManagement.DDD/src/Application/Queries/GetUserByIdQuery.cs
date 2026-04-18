// GetUserByIdQuery.cs - Application Query
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.

using UserManagement.DDD.Application.DTOs;

namespace UserManagement.DDD.Application.Queries;

/// <summary>
/// Query to get a user by ID
/// </summary>
public record GetUserByIdQuery(Guid UserId);