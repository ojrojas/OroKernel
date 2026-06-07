// UserManagement
// Copyright (C) 2026 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.

using Microsoft.EntityFrameworkCore;
using OroKernel.Shared.Entities;
using OroKernel.Shared.Interfaces;
using UserManagement.Data;

namespace UserManagement.BusinessRules;

public sealed class UserEmailMustBeUniqueRule : IBusinessRule
{
    private readonly string _email;
    private readonly UserManagementDbContext _dbContext;

    public UserEmailMustBeUniqueRule(string email, UserManagementDbContext dbContext)
    {
        _email = email ?? throw new ArgumentNullException(nameof(email));
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public Error? Error { get; private set; }

    public bool IsSatisfied()
    {
        var exists = _dbContext.Users.Any(u => u.Email == _email);
        if (exists)
        {
            Error = Error.Conflict("A user with this email already exists.");
            return false;
        }
        return true;
    }
}
