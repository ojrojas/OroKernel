// UserManagement
// Copyright (C) 2026 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.

using OroKernel.Shared.Entities;
using OroKernel.Shared.Interfaces;

namespace UserManagement.BusinessRules;

public sealed class MinimumUserNameLengthRule : IBusinessRule
{
    private readonly string _userName;

    public MinimumUserNameLengthRule(string userName)
    {
        _userName = userName ?? throw new ArgumentNullException(nameof(userName));
    }

    public Error? Error { get; private set; }

    public bool IsSatisfied()
    {
        if (_userName.Length < 3)
        {
            Error = Error.Validation("Username must be at least 3 characters long.");
            return false;
        }
        return true;
    }
}
