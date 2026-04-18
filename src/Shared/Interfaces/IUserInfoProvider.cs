// OroKernel
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.
namespace OroKernel.Shared.Interfaces;

/// <summary>
/// Provider abstraction to obtain current user information (per request)
/// </summary>
public interface IUserInfoProvider
{
    /// <summary>
    /// Get current user info; may return null if unavailable
    /// </summary>
    UserInfo? GetUserInfo();
}