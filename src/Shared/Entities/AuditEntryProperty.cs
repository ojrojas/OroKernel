// OroKernel
// Copyright (C) 2026 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.
namespace OroKernel.Shared.Entities;

/// <summary>
/// Represents a property change within an audit entry.
/// </summary>
public class AuditEntryProperty
{
    /// <summary>
    /// Gets or sets the unique identifier for the audit entry property.
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Gets or sets the identifier of the associated audit entry.
    /// </summary>
    public int AuditEntryId { get; set; }
    /// <summary>
    /// Gets or sets the name of the property that was changed.
    /// </summary>
    public string PropertyName { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the old value of the property before the change.
    /// </summary>
    public string? OldValue { get; set; }
    /// <summary>
    /// Gets or sets the new value of the property after the change.
    /// </summary>
    public string? NewValue { get; set; }
    /// <summary>
    /// Gets or sets the associated audit entry.
    /// </summary>
    public AuditEntry AuditEntry { get; set; } = null!;
}
