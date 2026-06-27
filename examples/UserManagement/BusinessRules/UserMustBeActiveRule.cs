// UserManagement
// Copyright (C) 2026 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.

using OroKernel.Domain.Entities;
using OroKernel.Domain.Interfaces;

namespace UserManagement.BusinessRules;

public sealed class UserMustBeActiveRule : IBusinessRule
{
    private readonly User _user;

    public UserMustBeActiveRule(User user)
    {
        _user = user ?? throw new ArgumentNullException(nameof(user));
    }

    public Error? Error { get; private set; }

    public bool IsSatisfied()
    {
        if (!_user.IsActive)
        {
            Error = Error.Validation("User account is deactivated.");
            return false;
        }
        return true;
    }
}
