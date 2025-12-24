// OroIdentityServer
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.
namespace OroKernel.Shared.Interfaces;

/// <summary>
/// Domain event interface
/// </summary>
public interface IDomainEvent : INotification
{
    /// <summary>
    /// Date and time when the event occurred
    /// </summary>
    DateTime OcurredOn { get; }
}
