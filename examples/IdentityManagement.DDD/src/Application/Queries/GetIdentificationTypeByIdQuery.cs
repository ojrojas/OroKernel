// GetIdentificationTypeByIdQuery.cs - Application Query
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.

using IdentityManagement.DDD.Application.DTOs;
using IdentityManagement.DDD.Domain.ValueObjects;

namespace IdentityManagement.DDD.Application.Queries;

/// <summary>
/// Query to get an identification type by ID
/// </summary>
public record GetIdentificationTypeByIdQuery(IdentificationTypeId Id);